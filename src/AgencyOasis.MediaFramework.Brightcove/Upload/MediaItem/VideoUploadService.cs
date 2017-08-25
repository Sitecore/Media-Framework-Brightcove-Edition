using AgencyOasis.MediaFramework.Brightcove.Entities;
using Sitecore.Data.Items;

namespace AgencyOasis.MediaFramework.Brightcove.Upload.MediaItem
{
    public class VideoUploadService : IVideoUploadService<VideoUploadServiceConfigBase>
    {
        public VideoUploadServiceConfigBase Config { get; set; }

        public VideoUploadExecutorBase GetExecutor(Item accountItem, UploadFileInfo uploadFileInfo)
        {
            return new VideoUploadExecutor(accountItem, uploadFileInfo, Config);
        }
    }
}