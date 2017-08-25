using AgencyOasis.MediaFramework.Brightcove.Entities;
using AgencyOasis.MediaFramework.Brightcove.Proxy.CMS;
using AgencyOasis.MediaFramework.Brightcove.Security;
using Sitecore.Data.Items;
using Sitecore.MediaFramework;
using Sitecore.MediaFramework.Export;
using Sitecore.MediaFramework.Synchronize;
using FieldIDs = AgencyOasis.MediaFramework.Brightcove.FieldIDs;

namespace AgencyOasis.MediaFramework.Brightcove.Export
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
            return item[Sitecore.MediaFramework.Brightcove.FieldIDs.MediaElement.Id].Length == 0;
        }
    }
}
