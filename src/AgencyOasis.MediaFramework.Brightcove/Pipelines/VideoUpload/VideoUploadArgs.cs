using AgencyOasis.MediaFramework.Brightcove.Entities;
using AgencyOasis.MediaFramework.Brightcove.Upload;
using Sitecore.Pipelines;

namespace AgencyOasis.MediaFramework.Brightcove.Pipelines.VideoUpload
{
    public class VideoUploadArgs : PipelineArgs
    {
        public VideoUploadExecutorBase Service { get; set; }

        public Video Video { get; set; }
    }
}
