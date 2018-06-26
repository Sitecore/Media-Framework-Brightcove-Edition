using System;
using Brightcove.MediaFramework.Brightcove.Entities;
using Brightcove.MediaFramework.Brightcove.Pipelines.VideoUpload;
using Sitecore.Data.Items;

namespace Brightcove.MediaFramework.Brightcove.Upload
{
    public static class VideoUploadManager
    {
        public static IVideoUploadService Service;

        static VideoUploadManager()
        {
            Service = Sitecore.Configuration.Factory.CreateObject("Brightcove.VideoUploadService", true) as
                IVideoUploadService;
        }

        public static Video Upload(Item accountItem, UploadFileInfo uploadFileInfo)
        {
            var args = new VideoUploadArgs
            {
                Service = Service.GetExecutor(accountItem, uploadFileInfo)
            };

            try
            {
                args.Service.Start();
                Sitecore.Pipelines.CorePipeline.Run("Brightcove.VideoUpload", args);
                args.Service.EndWithSuccess();
            }
            catch (Exception ex)
            {
                args.Service.EndWithError(ex);
                throw;
            }

            return args.Video;
        }
    }
}