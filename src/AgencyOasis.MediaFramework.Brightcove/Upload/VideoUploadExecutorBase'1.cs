using Brightcove.MediaFramework.Brightcove.Configuration;
using Brightcove.MediaFramework.Brightcove.Entities;
using Brightcove.MediaFramework.Brightcove.Upload.MediaItem;
using Sitecore.Data.Items;
using Sitecore.MediaFramework.Diagnostics;
using Sitecore.Mvc.Extensions;
using System.Collections.ObjectModel;
using System.Linq;

namespace Brightcove.MediaFramework.Brightcove.Upload
{
    public abstract class VideoUploadExecutorBase<TConfig, TSConfig> : VideoUploadExecutorBase
        where TConfig : VideoUploadServiceConfigBase
        where TSConfig : StorageServiceConfigBase
    {
        protected TConfig Config;
        protected StorageExecutorBase<TSConfig> StorageExecutor { get; set; }

        protected VideoUploadExecutorBase(Item accountItem, UploadFileInfo uploadFileInfo, TConfig config)
            : base(accountItem, uploadFileInfo)
        {
            Config = config;
            StorageExecutor = StorageServiceManager.Service.GetExecutor() as StorageExecutorBase<TSConfig>;
        }

        protected abstract string GetCallbackUrl();

        public override void CreateIngestVideoEntity()
        {
            var callback = Settings.IngestionCallbackUrl(StorageExecutor.Config.BaseUrl, GetCallbackUrl());

            IngestVideoEntity = new IngestVideo
            {
                VideoId = VideoEntity.Id,
                IngestMaster = new IngestMaster
                {
                    Url = UploadFileInfo.Url
                },
                CaptureImages = Config.CaptureImages,
                Profile = Config.Profile,
                Callbacks = new Collection<string> { callback }
            };

            LogHelper.Info(string.Format("ingesting for: VideoId={0}, url={1}, callback={2}", IngestVideoEntity.VideoId, IngestVideoEntity.IngestMaster.Url, IngestVideoEntity.Callbacks.FirstOrDefault()), this);
        }

        public override void IngestVideo()
        {
            base.IngestVideo();
            VideoEntity =  !VideoEntity.Id.IsEmptyOrNull() && (!VideoEntity.IngestJobId.IsEmptyOrNull() || Config.AlwaysCreateVideoItem) ? VideoEntity : null;
        }
    }
}