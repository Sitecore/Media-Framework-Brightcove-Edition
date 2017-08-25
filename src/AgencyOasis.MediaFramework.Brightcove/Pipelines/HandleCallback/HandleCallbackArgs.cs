using Brightcove.MediaFramework.Brightcove.Entities;
using Brightcove.MediaFramework.Brightcove.Upload;
using Sitecore.Pipelines;

namespace Brightcove.MediaFramework.Brightcove.Pipelines.HandleCallback
{
    public class HandleCallbackArgs : PipelineArgs
    {
        public Notification Notification { get; set; }

        public string OperationId { get; set; }
    }
}
