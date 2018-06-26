namespace Sitecore.MediaFramework.Upload
{
  using System.Web;

  /// <summary>
  /// Summary description for Handler1
  /// </summary>
  public class MediaUpload : IHttpHandler
  {
    public void ProcessRequest(HttpContext context)
    {
      UploadManager.HandleUploadRequest(context);
    }

    public bool IsReusable
    {
      get
      {
        return false;
      }
    } 
  }
}