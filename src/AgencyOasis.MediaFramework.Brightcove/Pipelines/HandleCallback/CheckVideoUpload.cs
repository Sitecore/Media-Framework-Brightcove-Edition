
using AgencyOasis.MediaFramework.Brightcove.Extensions;
using AgencyOasis.MediaFramework.Brightcove.Pipelines.VideoUpload;
using AgencyOasis.MediaFramework.Brightcove.Upload;

namespace AgencyOasis.MediaFramework.Brightcove.Pipelines.HandleCallback
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