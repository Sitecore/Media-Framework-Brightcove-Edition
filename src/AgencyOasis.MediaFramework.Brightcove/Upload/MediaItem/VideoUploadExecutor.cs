using AgencyOasis.MediaFramework.Brightcove.Configuration;
using AgencyOasis.MediaFramework.Brightcove.Entities;
using AgencyOasis.MediaFramework.Brightcove.Extensions;
using Sitecore.ContentSearch.Linq.Extensions;
using Sitecore.Data.Items;

namespace AgencyOasis.MediaFramework.Brightcove.Upload.MediaItem
{
    public class VideoUploadExecutor : VideoUploadExecutorBase<VideoUploadServiceConfigBase, StorageServiceConfig>
    {
        private Sitecore.Data.Items.MediaItem _mediaItem;

        public VideoUploadExecutor(Item accountItem, UploadFileInfo uploadFileInfo,
            VideoUploadServiceConfigBase config)
            : base
                (accountItem, uploadFileInfo, config)
        {
        }

        public override void SaveVideoContent()
        {
            _mediaItem = StorageExecutor.Save(UploadFileInfo) as Sitecore.Data.Items.MediaItem;
        }

        protected override string GetCallbackUrl()
        {
            return _mediaItem.InnerItem.Parent.ID.ToUrlString();
        }
    }
}