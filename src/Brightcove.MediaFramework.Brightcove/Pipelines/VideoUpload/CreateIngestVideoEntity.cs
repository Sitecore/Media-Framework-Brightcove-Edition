
namespace Brightcove.MediaFramework.Brightcove.Pipelines.VideoUpload
{
    public class CreateIngestVideoEntity : VideoUploadProcessor
    {
        public override void Process(VideoUploadArgs args)
        {
            args.Service.CreateIngestVideoEntity();
        }
    }
}