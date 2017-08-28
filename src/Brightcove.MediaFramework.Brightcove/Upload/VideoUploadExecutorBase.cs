using Brightcove.MediaFramework.Brightcove.Entities;
using Brightcove.MediaFramework.Brightcove.Proxy.CMS;
using Brightcove.MediaFramework.Brightcove.Security;
using Newtonsoft.Json;
using RestSharp;
using Sitecore.Data.Items;
using Sitecore.MediaFramework.Diagnostics;
using Sitecore.Mvc.Extensions;
using System;

namespace Brightcove.MediaFramework.Brightcove.Upload
{
    public abstract class VideoUploadExecutorBase
    {
        protected IAuthenticator Authenticator;
        protected UploadFileInfo UploadFileInfo;
        protected string PublisherId;
        public Video VideoEntity { get; protected set; }
        protected IngestVideo IngestVideoEntity { get; set; }
        public bool IsVideoPostSuccessful { get; protected set; }

        protected VideoUploadExecutorBase(Item accountItem, UploadFileInfo uploadFileInfo)
        {
            var account = new BrightcoveAuthenticator(accountItem);
            Authenticator = account;
            PublisherId = account.PublisherId;
            UploadFileInfo = uploadFileInfo;
        }

        public virtual void CreateVideoEntity()
        {
            VideoEntity = new Video
            {
                Name = UploadFileInfo.FileNameWithoutExtension,
                ShortDescription = UploadFileInfo.FileNameWithoutExtension
            };
        }

        public virtual void PostVideo()
        {
            if (VideoEntity != null && !VideoEntity.Name.IsEmptyOrNull())
            {
                var videoToUpload = VideoEntity;
                var proxy = new VideoProxy(Authenticator);
                VideoEntity = proxy.Create(VideoEntity);
                LogStep("CreateVideo", () => JsonConvert.SerializeObject(videoToUpload), () => JsonConvert.SerializeObject(VideoEntity));
                if (VideoEntity != null && !VideoEntity.Id.IsEmptyOrNull())
                {
                    IsVideoPostSuccessful = true;
                    UploadFileInfo.MediaId = VideoEntity.Id;
                    return;
                }
            }

            VideoEntity = null;
        }

        public abstract void SaveVideoContent();

        public abstract void CreateIngestVideoEntity();

        public virtual void IngestVideo()
        {
            var ingestRequest = IngestVideoEntity;
            var ingestProxy = new DynamicIngestProxy(Authenticator);
            IngestVideoEntity = ingestProxy.Ingest(IngestVideoEntity);
            if (IngestVideoEntity != null && !IngestVideoEntity.Id.IsEmptyOrNull())
            {
                LogHelper.Info(string.Format("Ingest details : VideoId={0}, ingest-JobId={1}", VideoEntity.Id, IngestVideoEntity.Id), this);

                LogStep("IngestVideo", () => JsonConvert.SerializeObject(ingestRequest), () => JsonConvert.SerializeObject(IngestVideoEntity));

                VideoEntity.IngestJobId = IngestVideoEntity.Id;
            }
        }

        public virtual void EndWithError(Exception ex)
        { }

        public virtual void Start()
        { }

        public virtual void EndWithSuccess()
        { }

        public virtual void LogStep(string action, Func<string> input, Func<string> output)
        {
        }
    }
}