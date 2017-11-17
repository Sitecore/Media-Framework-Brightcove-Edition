namespace Sitecore.MediaFramework.Upload
{
  using System;
  using System.Configuration.Provider;
  using System.Web;

  public abstract class UploadProviderBase : ProviderBase
  {
    public abstract void Update(Guid MediaItemId, Guid fileId, Guid accountId, byte progress, string error = null, bool canceled = false);
                                                                  
    public abstract FileUploadStatus GetStatus(Guid fileId);

    public abstract void HandleUploadRequest(HttpContext context);
  }
}