namespace Sitecore.MediaFramework.Brightcove.Export
{
  using Sitecore.MediaFramework.Brightcove.Entities;
  using Sitecore.MediaFramework.Brightcove.Security;
  using Sitecore.MediaFramework.Export;
  using Sitecore.RestSharp;

  public class VideoExporterWithInactivate : VideoExporter
  {
    protected override void Delete(ExportOperation operation)
    {
      var synchronizer = MediaFrameworkContext.GetItemSynchronizer(operation.Item);
      if (synchronizer == null)
      {
        return;
      }

      var video = (Video)synchronizer.CreateEntity(operation.Item);

      var inactivatedVideo = new Video
        {
          Id = video.Id,
          ItemState = ItemState.INACTIVE
        };

      var authenticator = new BrightcoveAthenticator(operation.AccountItem);

      var context = new RestContext(Constants.SitecoreRestSharpService, authenticator);

      var updateData = new PostData("update_video", authenticator, "video", inactivatedVideo);

      var data = context.Update<PostData, ResultData<Video>>("update_data", updateData).Data;
    }
  }
}