
using Brightcove.MediaFramework.Brightcove.Extensions;
using Brightcove.MediaFramework.Brightcove.Helpers;
using Brightcove.MediaFramework.Brightcove.Pipelines.VideoUpload;

namespace Brightcove.MediaFramework.Brightcove.Pipelines.HandleCallback
{
    public class SetIngestStatus : HandleCallbackProcessor
    {
        public override void Process(HandleCallbackArgs args)
        {
            if (args.Notification.IsVideoUpload())
            {
                VideoHelper.SetIngestStatus(args.Notification);
            }
        }
    }
}