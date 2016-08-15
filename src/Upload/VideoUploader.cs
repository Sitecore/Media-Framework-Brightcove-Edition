namespace Sitecore.MediaFramework.Brightcove.Upload
{
  using System;
  using System.Collections.Specialized;
  using System.Globalization;
  using System.IO;

  using Sitecore.MediaFramework.Diagnostics;

  using global::RestSharp;           

  using Sitecore.Data.Items; 
  using Sitecore.MediaFramework.Brightcove.Entities;              
  using Sitecore.MediaFramework.Brightcove.Security;
  using Sitecore.MediaFramework.Upload;
  using Sitecore.RestSharp;
  using Sitecore.RestSharp.Data;     

  public class VideoUploader : UploadExecuterBase
  {
    protected override object UploadInternal(NameValueCollection parameters, byte[] fileBytes, Item accountItem)
    {
      try
      {
        var fileName = this.GetFileName(parameters);
        string name = Path.GetFileNameWithoutExtension(fileName);

        var videoToUpload = new VideoToUpload
        {
          Name = name,
          ShortDescription = name,
          StartDate = ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString(CultureInfo.InvariantCulture)
        };

        var authenticator = new BrightcoveAthenticator(accountItem);

        var updateData = new PostData("create_video", authenticator, "video", videoToUpload);

        var context = new RestContext(Constants.SitecoreRestSharpService, authenticator);

        IRestRequest restRequest = context.CreateRequest<PostData, ResultData<string>>(EntityActionType.Create, "update_data", updateData);

        restRequest.AddFile(name, fileBytes, fileName);
        restRequest.Timeout = 86400000; // 24h

        var data = context.GetResponse<ResultData<string>>(restRequest).Data;

        if (data != null && !string.IsNullOrEmpty(data.Result))
        {
          return new Video { Id = data.Result, Name = name };
        }

        return null;
      }
      catch (Exception ex)
      {
        LogHelper.Error("Brightcove Upload is failed", this, ex);
        return null;
      }    
    } 
  }
}       
