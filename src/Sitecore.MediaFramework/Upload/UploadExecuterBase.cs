namespace Sitecore.MediaFramework.Upload
{
  using System;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.IO;
  using System.Linq;

  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Globalization;
  using Sitecore.MediaFramework.Synchronize;

  public abstract class UploadExecuterBase : IUploadExecuter
  {
    private List<string> fileExtensions;

    [Obsolete("Use MediaFrameworkContext.GetItemSynchronizer method")]   
    public IItemSynchronizer Synchronizer { get; set; }

    public string Extensions { get; set; }

    public bool SupportCanceling { get; set; }

    public List<string> FileExtensions
    {
      get
      {
        return this.fileExtensions ?? (this.fileExtensions = this.Extensions.Split(new[] { ',', '|' }, StringSplitOptions.RemoveEmptyEntries).ToList());
      }
    }

    public virtual void Upload(NameValueCollection parameters, byte[] fileBytes)
    {
      Item accountItem = this.GetAccountItem(parameters);

      if (accountItem != null)
      {
        if (!this.ValidateFileExtension(parameters.Get(Constants.Upload.FileName)))
        {
          this.UpdateStatus(Guid.Empty, this.GetFileId(parameters), accountItem.ID.Guid, 0, Translate.Text(Translations.FileUploadingWrongExtension + this.FileExtensions));
          return;
        }

        object entity = this.UploadInternal(parameters, fileBytes, accountItem);

        if (entity is CanceledVideo)
        {
          return;
        }

        if (entity != null)
        {
          var item = this.SyncItem(entity, accountItem);
          if (item != null)
          {
            this.UpdateStatus(item.ID.Guid, this.GetFileId(parameters), accountItem.ID.Guid, 100);       
            return;
          }
        }

        this.UpdateStatus(Guid.Empty, this.GetFileId(parameters), accountItem.ID.Guid, 0, Translate.Text(Translations.UploadingFailed));
      }
    }
        
    public virtual void Cancel(Guid fileId, Guid accountId)
    {
      UploadManager.Cancel(fileId, accountId);
    }

    public virtual bool ValidateFileExtension(string fileName)
    {
      var extension = Path.GetExtension(fileName);
      return extension != null && (!string.IsNullOrEmpty(fileName) && this.FileExtensions.Contains(extension.Remove(0, 1)));
    }

    protected abstract object UploadInternal(NameValueCollection parameters, byte[] fileBytes, Item accountItem);

    protected virtual Item SyncItem(object entity, Item accountItem)
    {
      var synchronizer = MediaFrameworkContext.GetItemSynchronizer(entity);

      return synchronizer != null ? synchronizer.SyncItem(entity, accountItem) : null;
    }

    protected virtual Item GetAccountItem(NameValueCollection parameters)
    {
      ID accountId = new ID(this.GetAccountId(parameters));

      Database db = Factory.GetDatabase(this.GetDatabase(parameters));

      return db.GetItem(accountId);                                  
    }

    protected virtual Guid GetAccountId(NameValueCollection parameters)
    {
      return new Guid(parameters[Constants.Upload.AccountId]);
    }

    protected virtual string GetDatabase(NameValueCollection parameters)
    {
      return parameters[Constants.Upload.Database];
    }

    [Obsolete("Use GetFileId method")]
    protected virtual Guid GetFieldId(NameValueCollection parameters)
    {
      return this.GetFileId(parameters);
    }

    protected virtual Guid GetFileId(NameValueCollection parameters)
    {
      return new Guid(parameters[Constants.Upload.FileId]);
    }
           
    protected virtual string GetFileName(NameValueCollection parameters)
    {
      return parameters[Constants.Upload.FileName];
    }

    protected virtual void UpdateStatus(Guid mediaItemId, Guid fileId, Guid accountId, byte progress, string error = null)
    {
      UploadManager.Update(mediaItemId, fileId, accountId, progress, error);
    }

    protected virtual bool IsCanceled(NameValueCollection parameters)
    {
      FileUploadStatus status;
      AccountUploadStatus accountStatus;
      var dd = (status = UploadManager.GetStatus(this.GetFileId(parameters))) != null
              && (accountStatus = status.ProgressList.FirstOrDefault(st => st.AccountId == this.GetAccountId(parameters))) != null
              && accountStatus.Canceled;

      return dd;
    }
  }
}