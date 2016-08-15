namespace Sitecore.MediaFramework.Brightcove.Export
{
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Xml;

  using Sitecore.Configuration;
  using Sitecore.MediaFramework.Brightcove.Entities;
  using Sitecore.MediaFramework.Brightcove.Security;
  using Sitecore.MediaFramework.Export;
  using Sitecore.RestSharp;

  public class VideoExporterWithDelete : VideoExporter, IConstructable
  {
    public string DeleteParams { get; set; }
    
    protected NameValueCollection DeleteParameters = new NameValueCollection();

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="operation"></param>
    protected override void Delete(ExportOperation operation)
    {
      var synchronizer = MediaFrameworkContext.GetItemSynchronizer(operation.Item);
      if (synchronizer == null)
      {
        return;
      }

      var video = (Video)synchronizer.CreateEntity(operation.Item);

      var parameters = new Dictionary<string, object>
        {
          { "video_id", video.Id },
          { "cascade", this.DeleteParameters["cascade"] ?? "true" },
          { "delete_shares", this.DeleteParameters["delete_shares"] ?? "true"}
        };

      var authenticator = new BrightcoveAthenticator(operation.AccountItem);

      var updateData = new PostData("delete_video", authenticator, parameters);

      var context = new RestContext(Constants.SitecoreRestSharpService, authenticator);

      context.Delete<PostData, ResultData<PlayList>>("update_data", updateData);
    }

    public virtual void Constructed(XmlNode configuration)
    {
      this.DeleteParameters = StringUtil.GetNameValues(this.DeleteParams);
    }
  }
}