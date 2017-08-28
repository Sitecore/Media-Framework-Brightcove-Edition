
namespace Brightcove.MediaFramework.Brightcove.Pipelines.VideoUpload
{
    public class PostVideo : VideoUploadProcessor
    {
        public override void Process(VideoUploadArgs args)
        {
            args.Service.PostVideo();
        }
    }
}