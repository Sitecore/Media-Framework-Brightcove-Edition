using Brightcove.MediaFramework.Brightcove.Entities;
using Sitecore.Data.Items;

namespace Brightcove.MediaFramework.Brightcove.Upload.MediaItem
{
    public class StorageService : IStorageService<StorageServiceConfig>
    {
        public StorageServiceConfig Config { get; set; }
        
        public StorageExecutorBase GetExecutor()
        {
            return new StorageExecutor(Config);
        }
    }
}