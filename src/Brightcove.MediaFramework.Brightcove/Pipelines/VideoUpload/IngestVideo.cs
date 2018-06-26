
namespace Brightcove.MediaFramework.Brightcove.Pipelines.VideoUpload
{
    public class IngestVideo : VideoUploadProcessor
    {
        public override void Process(VideoUploadArgs args)
        {
            args.Service.IngestVideo();
            args.Video = args.Service.VideoEntity;
        }
    }
}