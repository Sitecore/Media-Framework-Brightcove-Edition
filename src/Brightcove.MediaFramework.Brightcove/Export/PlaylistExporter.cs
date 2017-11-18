using Brightcove.MediaFramework.Brightcove.Entities;
using Brightcove.MediaFramework.Brightcove.Proxy.CMS;
using Brightcove.MediaFramework.Brightcove.Security;
using Sitecore.Data.Items;
using Sitecore.MediaFramework;
using Sitecore.MediaFramework.Export;

namespace Brightcove.MediaFramework.Brightcove.Export
{
    public class PlaylistExporter : ExportExecuterBase
    {
        protected override object Create(ExportOperation operation)
        {
            var itemSynchronizer = MediaFrameworkContext.GetItemSynchronizer(operation.Item);
            if (itemSynchronizer == null)
                return null;
            var playList = (PlayList)itemSynchronizer.CreateEntity(operation.Item);
            playList.Id = null;
            playList.ReferenceId = null;
            //Video ids used only for EXPLICIT playlist.
            //In other case will be used playlistsearch
            if (playList.PlaylistType == PlaylistType.EXPLICIT.ToString())
            {
                playList.PlayListSearch = null;
            }
            else
                playList.VideoIds = null;
            var authenticator = new BrightcoveAuthenticator(operation.AccountItem);

            var result = new PlaylistProxy(authenticator).Create(playList);
            if (result == null || string.IsNullOrEmpty(result.Id))
                return null;
            playList.Id = result.Id;
            return playList;
        }

        protected override void Delete(ExportOperation operation)
        {
            var itemSynchronizer = MediaFrameworkContext.GetItemSynchronizer(operation.Item);
            if (itemSynchronizer == null)
                return;
            var authenticator = new BrightcoveAuthenticator(operation.AccountItem);
            new PlaylistProxy(authenticator).Delete(((Asset)itemSynchronizer.CreateEntity(operation.Item)).Id);
        }

        protected override object Update(ExportOperation operation)
        {
            var itemSynchronizer = MediaFrameworkContext.GetItemSynchronizer(operation.Item);
            if (itemSynchronizer == null)
                return null;
            var playList = (PlayList)itemSynchronizer.CreateEntity(operation.Item);
            playList.ReferenceId = null;
            //Video ids used only for EXPLICIT playlist.
            //In other case will be used playlistsearch
            if (playList.PlaylistType == PlaylistType.EXPLICIT.ToString())
            {
                playList.PlayListSearch = null;
            }
            else
                playList.VideoIds = null;
            var authenticator = new BrightcoveAuthenticator(operation.AccountItem);
            var data = new PlaylistProxy(authenticator).Patch(playList);

            if (data == null)
                return null;
            return data;
        }

        public override bool IsNew(Item item)
        {
            return item[FieldIDs.MediaElement.Id].Length == 0;
        }
    }
}
