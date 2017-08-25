using AgencyOasis.MediaFramework.Brightcove.Entities;
using AgencyOasis.MediaFramework.Brightcove.Upload;
using Sitecore.Pipelines;

namespace AgencyOasis.MediaFramework.Brightcove.Pipelines.HandleCallback
{
    public class HandleCallbackArgs : PipelineArgs
    {
        public Notification Notification { get; set; }

        public string OperationId { get; set; }
    }
}
