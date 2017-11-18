using Brightcove.MediaFramework.Brightcove.Entities;
using Brightcove.MediaFramework.Brightcove.Proxy.CMS;
using Brightcove.MediaFramework.Brightcove.Security;
using Sitecore.Data.Items;
using Sitecore.MediaFramework;
using Sitecore.MediaFramework.Export;
using Sitecore.MediaFramework.Synchronize;
using FieldIDs = Brightcove.MediaFramework.Brightcove.FieldIDs;

namespace Brightcove.MediaFramework.Brightcove.Export
{
    /// <summary>
    /// Video Exporter
    /// </summary>
    public abstract class VideoExporter : ExportExecuterBase
    {
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        protected override object Create(ExportOperation operation)
        {
            return null;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        protected override object Update(ExportOperation operation)
        {
            IItemSynchronizer itemSynchronizer = MediaFrameworkContext.GetItemSynchronizer(operation.Item);
            if (itemSynchronizer == null)
                return null;
            var video = (Video)itemSynchronizer.CreateEntity(operation.Item);
            if (video.CustomFields != null && video.CustomFields.Count == 0)
                video.CustomFields = null;
            var authenticator = new BrightcoveAuthenticator(operation.AccountItem);
            var result = new VideoProxy(authenticator).Patch(video);
            if (result == null || result.Id == null)
                return null;
            if (result.CustomFields == null)
                result.CustomFields = video.CustomFields;
            return result;
        }

        /// <summary>
        /// Is New
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool IsNew(Item item)
        {
            return item[FieldIDs.MediaElement.Id].Length == 0;
        }
    }
}
