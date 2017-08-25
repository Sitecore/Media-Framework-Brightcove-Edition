using AgencyOasis.MediaFramework.Brightcove.Entities;
using Sitecore.Data.Items;
using Sitecore.Syndication;

namespace AgencyOasis.MediaFramework.Brightcove.Upload
{
    public interface IVideoUploadService
    {
        VideoUploadExecutorBase GetExecutor(Item accountItem, UploadFileInfo uploadFileInfo);
    }
}