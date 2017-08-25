using Brightcove.MediaFramework.Brightcove.Entities;
using Sitecore.Data.Items;
using Sitecore.Syndication;

namespace Brightcove.MediaFramework.Brightcove.Upload
{
    public interface IVideoUploadService
    {
        VideoUploadExecutorBase GetExecutor(Item accountItem, UploadFileInfo uploadFileInfo);
    }
}