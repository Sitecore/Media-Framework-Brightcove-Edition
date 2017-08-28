
namespace Brightcove.MediaFramework.Brightcove.Pipelines.VideoUpload
{
    public class SaveVideoContent : VideoUploadProcessor
    {
        public override void Process(VideoUploadArgs args)
        {
            if (!args.Service.IsVideoPostSuccessful)
            {
                args.AbortPipeline();
                return;
            }

            args.Service.SaveVideoContent();
        }
    }
}