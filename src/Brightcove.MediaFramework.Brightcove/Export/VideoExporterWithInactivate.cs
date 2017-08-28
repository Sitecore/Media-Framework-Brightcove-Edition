using Brightcove.MediaFramework.Brightcove.Entities;
using Brightcove.MediaFramework.Brightcove.Proxy.CMS;
using Brightcove.MediaFramework.Brightcove.Security;
using Sitecore.MediaFramework;
using Sitecore.MediaFramework.Export;
using Sitecore.MediaFramework.Synchronize;

namespace Brightcove.MediaFramework.Brightcove.Export
{
  public class VideoExporterWithInactivate : VideoExporter
  {
    protected override void Delete(ExportOperation operation)
    {
      IItemSynchronizer itemSynchronizer = MediaFrameworkContext.GetItemSynchronizer(operation.Item);
      if (itemSynchronizer == null)
        return;
      var video1 = (Video) itemSynchronizer.CreateEntity(operation.Item);
      var video2 = new Video();
      video2.Id = video1.Id;
      video2.ItemState = ItemState.INACTIVE;
      Video video3 = video2;
      var authenticator = new BrightcoveAuthenticator(operation.AccountItem);
        new VideoProxy(authenticator).Patch(video3);
    }
  }
}
