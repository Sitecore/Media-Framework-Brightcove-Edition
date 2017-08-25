using Brightcove.MediaFramework.Brightcove.Entities;
using Brightcove.MediaFramework.Brightcove.Extensions;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.MediaFramework.Diagnostics;
using Sitecore.MediaFramework.Upload;
using System;
using System.Collections.Specialized;

namespace Brightcove.MediaFramework.Brightcove.Upload
{
    public class VideoUploader : UploadExecuterBase
    {
        protected override object UploadInternal(NameValueCollection parameters, byte[] fileBytes, Item accountItem)
        {
            try
            {
                string fileName = GetFileName(parameters);
                string fileId = GetFileId(parameters).ToString();

                var video = VideoUploadManager.Upload(accountItem, new UploadFileInfo{Bytes=fileBytes, Name = fileName, Id = ID.NewID.ToUrlString()});

                if (video == null)
                    return null;

                return video;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Brightcove Upload is failed", this, ex);
                return null;
            }
        }
    }
}