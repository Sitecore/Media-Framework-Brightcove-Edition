using System.Collections.Generic;
using System.Linq;
using Brightcove.MediaFramework.Brightcove.Configuration;
using Brightcove.MediaFramework.Brightcove.Entities;
using Brightcove.MediaFramework.Brightcove.Extensions;
using Sitecore.Data.Items;

namespace Brightcove.MediaFramework.Brightcove.Upload.MediaItem
{
    public class StorageExecutor : StorageExecutorBase<StorageServiceConfig>
    {
        public StorageExecutor(StorageServiceConfig config)
            : base (config)
        {
        }

        public override IEnumerable<object> Save(IList<UploadFileInfo> uploadFiles)
        {
            var result = base.Save(uploadFiles);

            var publishItem = FileStorageManager.ResolvePublishItem((result.FirstOrDefault() as Sitecore.Data.Items.MediaItem).InnerItem, Config.PublishStop, Config.PublishDatabase);
            FileStorageManager.Publish(publishItem, Config.PublishDatabase);

            return result;
        }

        protected override object SaveFile(UploadFileInfo uploadFileInfo)
        {
            var rootItem = FileStorageManager.VerifyRoot(Config.RootItem, Config.MediaFolderTemplate);
            var mediaItem = FileStorageManager.AddFile(Config.ContentDatabase, uploadFileInfo, string.Format(rootItem.Paths.ContentPath + "/" + uploadFileInfo.Id));

            uploadFileInfo.Url=Settings.FileDownloadUrl(Config.BaseUrl, mediaItem.InnerItem.ID.ToUrlString());

            return mediaItem;
        }

        public override UploadFileInfo RetrieveFile(string fileId)
        {
            return FileStorageManager.RetrieveFile(fileId, Config.PublishDatabase);
        }

        public override bool Delete(string itemId)
        {
            var root = FileStorageManager.VerifyRoot(Config.RootItem, Config.MediaFolderTemplate);
            FileStorageManager.DeleteMediaItem(root, itemId, Config.ContentDatabase, Config.PublishDatabase);
            return true;
        }
    }
}