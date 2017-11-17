namespace Sitecore.MediaFramework.Upload
{
  using System;
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.IO;
  using System.Linq;
  using System.Web;

  using Newtonsoft.Json;

  using Sitecore.Data;
  using Sitecore.Diagnostics;
  using Sitecore.Globalization;
  using Sitecore.Jobs;
  using Sitecore.MediaFramework.Common;
  using Sitecore.MediaFramework.Diagnostics;

  public class UploadProvider : UploadProviderBase
  {   
    protected ConcurrentDictionary<Guid, FileUploadStatus> StatusList { get; set; }

    protected Dictionary<string, Func<HttpContext, string>> actionList;
   
    public Dictionary<string, Func<HttpContext, string>> ActionList
    {
      get
      {
        return actionList ?? (actionList = this.InitActions());
      }
    }

    public bool IsReusable
    {
      get
      {
        return false;
      }
    }   

    public UploadProvider()
    {
      this.StatusList = new ConcurrentDictionary<Guid, FileUploadStatus>();
    }

    public override void Update(Guid mediaItemId, Guid fileId, Guid accountId, byte progress, string error = null, bool canceled = false)
    {
      progress = Math.Min(progress, (byte)100);

      var tmp = new FileUploadStatus
            {
              FileId = fileId,
            };
      tmp.ProgressList.Add(new AccountUploadStatus(mediaItemId, accountId, progress, error, canceled));

      this.StatusList.AddOrUpdate(
        fileId,
        tmp,
        (guid, status) =>
        {
          var accountData = status.ProgressList.FirstOrDefault(i => i.AccountId == accountId);

          if (accountData != null)
          {
            if (accountData.Canceled)
            {                                                                  
              return status;
            }

            accountData.Progress = progress;
            accountData.MediaItemId = mediaItemId;              
            accountData.Error = error;
            accountData.Canceled = canceled;
          }
          else
          {
            status.ProgressList.Add(new AccountUploadStatus { AccountId = accountId, Progress = progress, MediaItemId = mediaItemId, Error = error, Canceled = canceled });
          }

          return status;
        });
    }

    public override FileUploadStatus GetStatus(Guid fileId)
    {
      FileUploadStatus status;
      this.StatusList.TryRemove(fileId, out status);

      return status;
    }

    #region HttpHandler

    public override void HandleUploadRequest(HttpContext context)
    {
      var rt = context.Request.QueryString.Get(Constants.Upload.RequestType);
      if (!string.IsNullOrEmpty(rt) && this.ActionList.ContainsKey(rt))
      {
        var funct = this.ActionList[rt];
        context.Response.Write(funct(context));
        return;
      }

      LogHelper.Warn("Wrong request came to the Upload handler.", this);
    }

    protected virtual Dictionary<string, Func<HttpContext, string>> InitActions()
    {
      var actions = new Dictionary<string, Func<HttpContext, string>>
                      {
                        { Constants.Upload.StartUpload, this.Upload },
                        { Constants.Upload.Progress, this.GetUploadStatus },     
                        { Constants.Upload.Goto, this.Goto }, 
                      };
      return actions;
    }

    protected virtual string Goto(HttpContext context)
    {
      bool isId;
      var id = this.GetQueryStringID(context, Constants.Upload.MediaItemId, "Goto Item functionality failed. Item Id property is not defined or is in incorrect format", out isId);
      var db = this.GetQueryStringProperty(context, Constants.Upload.Database, "Goto functionality failed. Database property is not defined");

      if (isId && db.Length > 0)
      {
        Context.ClientPage.SendMessage(this, "item:load(id=" + id + ", db= " + db + ")");
      }

      return string.Empty;
    }
  
    protected virtual string GetUploadStatus(HttpContext context)
    {
      bool isId;
      Guid fileId = this.GetQueryStringID(context, Constants.Upload.Id, "Get Upload Status functionality failed. Upload File Id property is not defined or is in incorrect format", out isId);

      FileUploadStatus status = null;
      if (isId)
      {
        status = UploadManager.GetStatus(fileId);
      }

      if (status == null)
      {
        return this.HandleNullStatus(context, fileId);
      }

      return JsonConvert.SerializeObject(status);
    }

    protected virtual string HandleNullStatus(HttpContext context, Guid fileId)
    {
      var accounts = context.Request.QueryString.Get("accounts");
      if (string.IsNullOrEmpty(accounts))
      {
        return JsonConvert.SerializeObject(null);
      }

      var accList = accounts.Split(',');

      foreach (var account in accList)
      {
        Guid accId;
        if (!Guid.TryParse(account, out accId))
        {
          this.GetQueryStringProperty(context, "accounts", "Get Upload Status functionality warning. Accounts id is in incorrect format");
          continue;
        }

        var name = string.Format("MediaFramework_Upload_{0}_{1}", accId, fileId);
        var job = JobManager.GetJob(name);

        if (job == null)
        {
          var st = new AccountUploadStatus
          {
            AccountId = accId,
            Progress = 0,
            Error = Translate.Text(Translations.UploadJobStopped)
          };

          LogHelper.Warn("The uploading process job is stopped, but video can be uploaded to the external server.", this);
          return JsonConvert.SerializeObject(new FileUploadStatus(fileId)
          {
            ProgressList = new List<AccountUploadStatus> { st }
          });
        }
      }

      return JsonConvert.SerializeObject(null);
    }

    protected virtual string Upload(HttpContext context)
    {
      context.Response.ContentType = "text/plain";
      var responce = new UploadingFile
      {
        Size = 0,
        ThumbnailUrl = string.Format(Constants.ErrorImage, 80, 80),
        ID = Guid.NewGuid()
      };

      try
      {
        if (context.Request.Files.Count == 0)
        {
          responce.Name = Translate.Text(Translations.NoFiles);
          responce.Error = Translate.Text(Translations.EmptyFileUploadResult);
          LogHelper.Warn("Upload is Stopped. Request does not contain any file", this);
        }
        else
        {
          var list = this.GetAccountList(context);
          if (!this.VerifyList(list))
          {
            responce.Error = Translate.Text(Translations.AccountWasNotSelected);
            LogHelper.Warn(string.Format("Upload is Stopped. {0}", Translations.AccountWasNotSelected), this);
          }
          else
          {
            var uploadedfile = context.Request.Files[0];
            var stream = uploadedfile.InputStream;
            var lenght = uploadedfile.ContentLength;

            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
              stream.CopyTo(memoryStream);
              fileBytes = memoryStream.ToArray();
            }

            foreach (Account account in list)
            {
              var uploadParameters = new NameValueCollection
                                       {
                                         {
                                           Constants.Upload.FileName,
                                           Path.GetFileName(uploadedfile.FileName)
                                         },
                                         { Constants.Upload.FileId, responce.ID.ToString() },
                                         { Constants.Upload.AccountId, account.ID.ToString() },
                                         {
                                           Constants.Upload.AccountTemplateId,
                                           account.AccountTemplateId.ToString()
                                         },
                                         {
                                           Constants.Upload.Database,
                                           context.Request.QueryString.Get(
                                             Constants.Upload.Database)
                                         }
                                       };

              var uploadProcess = new UploadProcess(uploadParameters, fileBytes);
              var name = string.Format("MediaFramework_Upload_{0}_{1}", account.ID, responce.ID);
              var options = new JobOptions(name, "MediaFramework", Context.Site.Name, uploadProcess, "Execute");

              var job = new Job(options);
              JobManager.Start(job);
            }

            responce.Name = uploadedfile.FileName;
            responce.Size = lenght;
            responce.ThumbnailUrl = string.Format(Constants.DefaultPreview, 80, 80);
          }
        }

        return JsonConvert.SerializeObject(responce);
      }
      catch (Exception ex)
      {
        LogHelper.Error("Upload is failed.", this, ex);
        return JsonConvert.SerializeObject(responce);
      }
    }

    protected virtual List<Account> GetAccountList(HttpContext context)
    {
      var list = new List<Account>();  
      var accountsStr = context.Request.QueryString.Get("accounts");
      string[] accountIds;
      if (!string.IsNullOrEmpty(accountsStr) && (accountIds = accountsStr.Split(',')).Length > 0)
      {
        list.AddRange(accountIds.Select(this.GetAccount).Where(acc => acc != null));
        return list;
      }

      LogHelper.Error("Upload is failed. Accounts are not defined", this);
      return list;
    }

    protected virtual Account GetAccount(string ids)
    {
      var accIds = ids.Split('|');
      if (accIds.Length == 2 && ID.IsID(accIds[0]) && ID.IsID(accIds[1]))
      {
        return new Account { ID = new Guid(accIds[0]), AccountTemplateId = new Guid(accIds[1]) };
      }

      LogHelper.Warn("Upload to one Account is canceled. Account id is in incorrect format", this);
      return null;
    }

    protected virtual bool VerifyList(List<Account> ids)
    {
      if (ids == null || ids.Count == 0)
      {
        LogHelper.Warn("Upload is failed. Accounts are not defined", this);
        return false;
      }

      return true;
    }
     
    protected virtual string GetQueryStringProperty(HttpContext context, string key, string errorMessage)
    {
      var param = context.Request.QueryString.Get(key);
      if (string.IsNullOrEmpty(param))
      {
        LogHelper.Error(errorMessage, this);
      }

      return param;
    }

    protected virtual Guid GetQueryStringID(HttpContext context, string key, string errorMessage, out bool isId)
    {
      var param = context.Request.QueryString.Get(key);
      isId = true;
      Guid id;
      if (!Guid.TryParse(param, out id))
      {
        isId = false;
        LogHelper.Error(errorMessage, this);
      }

      return id;
    }

    #endregion
  }
}