using System;
using System.Collections.Generic;
using System.Linq;
using Brightcove.MediaFramework.Brightcove.Entities;
using Brightcove.MediaFramework.Brightcove.Pipelines.VideoUpload;
using Sitecore.Data.Items;
using Sitecore.MediaFramework.Diagnostics;

namespace Brightcove.MediaFramework.Brightcove.Upload
{
    public interface IStorageService
    {
        StorageExecutorBase GetExecutor();
    }

    public interface IStorageService<out TConfig> : IStorageService where TConfig : StorageServiceConfigBase
    {
        TConfig Config { get; }
    }

    public abstract class StorageExecutorBase
    {
        public abstract UploadFileInfo RetrieveFile(string fileId);

        public virtual object Save(UploadFileInfo uploadFileInfo)
        {
            return Save(new List<UploadFileInfo> { uploadFileInfo }).FirstOrDefault();
        }

        public virtual IEnumerable<object> Save(IList<UploadFileInfo> uploadFiles)
        {
            return uploadFiles.Select(SaveFile).ToList();
        }

        protected abstract object SaveFile(UploadFileInfo uploadFileInfo);

        public abstract bool Delete(string itemId);
    }

    public abstract class StorageExecutorBase<TConfig> : StorageExecutorBase
        where TConfig : StorageServiceConfigBase
    {
        public TConfig Config;

        protected StorageExecutorBase(TConfig config)
        {
            Config = config;
        }
    }

    public static class StorageServiceManager
    {
        public static IStorageService Service;

        static StorageServiceManager()
        {
            Service = Sitecore.Configuration.Factory.CreateObject("Brightcove.StorageService", true) as
                IStorageService;
        }

        public static UploadFileInfo RetrieveFile(string fileId)
        {
            return Service.GetExecutor().RetrieveFile(fileId);
        }

        public static bool Delete(string requestId, Notification notification)
        {
            return Service.GetExecutor().Delete(requestId);
        }
    }
}