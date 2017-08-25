
using AgencyOasis.MediaFramework.Brightcove.Extensions;
using AgencyOasis.MediaFramework.Brightcove.Helpers;
using AgencyOasis.MediaFramework.Brightcove.Pipelines.VideoUpload;

namespace AgencyOasis.MediaFramework.Brightcove.Pipelines.HandleCallback
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