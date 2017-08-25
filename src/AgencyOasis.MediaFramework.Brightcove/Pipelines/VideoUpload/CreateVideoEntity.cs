
namespace AgencyOasis.MediaFramework.Brightcove.Pipelines.VideoUpload
{
    public class CreateVideoEntity : VideoUploadProcessor
    {
        public override void Process(VideoUploadArgs args)
        {
            args.Service.CreateVideoEntity();
        }
    }
}