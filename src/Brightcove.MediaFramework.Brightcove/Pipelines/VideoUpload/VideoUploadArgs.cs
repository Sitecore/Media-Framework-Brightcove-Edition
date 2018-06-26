using Brightcove.MediaFramework.Brightcove.Entities;
using Brightcove.MediaFramework.Brightcove.Upload;
using Sitecore.Pipelines;

namespace Brightcove.MediaFramework.Brightcove.Pipelines.VideoUpload
{
    public class VideoUploadArgs : PipelineArgs
    {
        public VideoUploadExecutorBase Service { get; set; }

        public Video Video { get; set; }
    }
}
