
using Brightcove.MediaFramework.Brightcove.Extensions;
using Brightcove.MediaFramework.Brightcove.Pipelines.VideoUpload;
using Brightcove.MediaFramework.Brightcove.Upload;

namespace Brightcove.MediaFramework.Brightcove.Pipelines.HandleCallback
{
    public class CheckVideoUpload : HandleCallbackProcessor
    {
        public override void Process(HandleCallbackArgs args)
        {
            if (args.Notification.IsVideoUpload() || args.Notification.IsAssetUpload())
            {
                if (args.Notification.IsSuccess())
                {
                    StorageServiceManager.Delete(args.OperationId, args.Notification);
                }
            }
        }
    }
}