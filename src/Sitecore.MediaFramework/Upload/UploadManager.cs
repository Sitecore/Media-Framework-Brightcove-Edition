namespace Sitecore.MediaFramework.Upload
{
  using System;
  using System.Web;

  using Sitecore.Configuration;
  using Sitecore.Integration.Common.Providers;

  public class UploadManager
  {
    #region Initialization

    static UploadManager()
    {
      var helper = new ProviderHelper<UploadProviderBase, ProviderCollection<UploadProviderBase>>("mediaFramework/uploadManager");
      Providers = helper.Providers;
      Provider = helper.Provider;
    }

    public static UploadProviderBase Provider { get; set; }

    public static ProviderCollection<UploadProviderBase> Providers { get; private set; }

    #endregion

    public static void Update(Guid mediaItemId, Guid fileId, Guid accountId, byte progress, string error = null)
    {
      Provider.Update(mediaItemId, fileId, accountId, progress, error);
    }

    public static void Cancel(Guid fileId, Guid accountId)
    {
      Provider.Update(Guid.Empty, fileId, accountId, 0, string.Empty, true);
    }

    public static FileUploadStatus GetStatus(Guid fileId)
    {
      return Provider.GetStatus(fileId);
    }

    public static void HandleUploadRequest(HttpContext context)
    {
      Provider.HandleUploadRequest(context);
    }
  }
}