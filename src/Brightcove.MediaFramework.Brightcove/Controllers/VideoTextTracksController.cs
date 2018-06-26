using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Brightcove.MediaFramework.Brightcove.Configuration;
using Brightcove.MediaFramework.Brightcove.Entities;
using Brightcove.MediaFramework.Brightcove.Entities.ViewModels;
using Brightcove.MediaFramework.Brightcove.Extensions;
using Brightcove.MediaFramework.Brightcove.Proxy.CMS;
using Brightcove.MediaFramework.Brightcove.Security;
using Brightcove.MediaFramework.Brightcove.Upload;
using Brightcove.MediaFramework.Brightcove.Upload.MediaItem;
using Sitecore.Data;
using System.Linq;
using System.Web.Mvc;
using Sitecore.Data.Items;
using Sitecore.MediaFramework.Diagnostics;

namespace Brightcove.MediaFramework.Brightcove.Controllers
{
    public class VideoTextTracksController : Controller
    {
        [HttpGet]
        public ActionResult TextTracks(string accountItemId, string videoId)
        {
            var model = CreateModel(accountItemId, videoId);
            return View("~/sitecore modules/Web/MediaFramework/MVC/Views/Brightcove/VideoTextTracks.cshtml", model);
        }

        [HttpPost]
        public ActionResult TextTracks(string accountItemId, string videoId, VideoTextTracks videoTextTracks)
        {
            var accountItem = Sitecore.Context.Database.GetItem(new ID(accountItemId));
            var authenticator = new BrightcoveAuthenticator(accountItem);
            var proxy = new VideoProxy(authenticator);
            bool uploadingTracks = false;
            if (videoTextTracks.Tracks != null && videoTextTracks.Tracks.Count > 0)
            {
                var anyToSaveOrUpload = videoTextTracks.Tracks.Any(i => !i.IsDelete);
                var anyFileUpload = videoTextTracks.Tracks.Any(i => i.File != null);
                var tracksToIngest = new List<IngestTextTrack>();
                var tracksToSave = new List<VideoTextTrack>();
                var callbackUrl = string.Empty;

                if (anyToSaveOrUpload)
                {
                    if (anyFileUpload)
                    {
                        var stringId = ID.NewID.ToUrlString();
                        var storageExecutor = StorageServiceManager.Service.GetExecutor() as StorageExecutor;
                        var files =
                            videoTextTracks.Tracks.Where(i => i.File != null).Select(i =>
                                new
                                {
                                    key = i,
                                    file = new UploadFileInfo
                                    {
                                        Name = i.File.FileName,
                                        Bytes = new BinaryReader(i.File.InputStream).ReadBytes(i.File.ContentLength),
                                        Id = stringId
                                    }
                                }).ToDictionary(a => a.key, a => a.file);

                        var objectItem = StorageServiceManager.Service.GetExecutor().Save(files.Values.ToList()).FirstOrDefault();

                        (from i in videoTextTracks.Tracks
                         from j in files
                         where i == j.Key
                         select MapFileUploadInfo(i, j.Value)).ToList();

                        if (objectItem != null)
                        {
                            callbackUrl = Settings.IngestionCallbackUrl(storageExecutor.Config.BaseUrl, ((MediaItem)objectItem).InnerItem.Parent.ID.ToUrlString());
                        }
                    }

                    tracksToSave = videoTextTracks.Tracks.Where(i => !i.IsDelete && !i.IsUpload).Select(MapToVideoTextTrack).ToList();
                    tracksToIngest = videoTextTracks.Tracks.Where(i => !i.IsDelete && i.IsUpload).Select(MapToIngestTextTrack).ToList();
                }

                proxy.UpdateTextTracks(videoId, tracksToSave);

                if (tracksToIngest.Count > 0)
                {
                    var ingestProxy = new DynamicIngestProxy(authenticator);
                    ingestProxy.Ingest(new IngestVideo
                    {
                        VideoId = videoId,
                        TextTracks = new Collection<IngestTextTrack>(tracksToIngest),
                        Callbacks = string.IsNullOrEmpty(callbackUrl) ? null : new Collection<string> { callbackUrl }
                    });
                    uploadingTracks = true;
                }
            }

            var model = CreateModel(proxy, videoId, uploadingTracks);
            return View("~/sitecore modules/Web/MediaFramework/MVC/Views/Brightcove/VideoTextTracks.cshtml", model);
        }

        private bool MapFileUploadInfo(TextTrackInfo i, UploadFileInfo j)
        {
            i.Src = j.Url;
            i.IsUpload = true;
            return true;
        }

        private static VideoTextTracks CreateModel(string accountItemId, string videoId)
        {
            var accountItem = Sitecore.Context.Database.GetItem(new ID(accountItemId));
            var authenticator = new BrightcoveAuthenticator(accountItem);
            var proxy = new VideoProxy(authenticator);
            return CreateModel(proxy, videoId);
        }

        private static VideoTextTracks CreateModel(VideoProxy proxy, string videoId, bool showUploading = false)
        {
            var video = proxy.RetrieveById(videoId);

            var tracks = video.TextTracks != null && video.TextTracks.Count > 0
                ? video.TextTracks.Select(MapToViewModel).ToList()
                : new List<TextTrackInfo>();

            return new VideoTextTracks
            {
                Video = video,
                KindList = new List<string> { "captions" },
                Languages = new List<string> { "ar", "et" },
                Tracks = tracks,
                UploadingTracks = showUploading
            };
        }

        private static TextTrackInfo MapToViewModel(VideoTextTrack value)
        {
            return new TextTrackInfo
            {
                Default = value.Default,
                Kind = value.Kind,
                Label = value.Label,
                MimeType = value.MimeType,
                Src = value.Src,
                SrcLang = value.SrcLang
            };
        }

        private static VideoTextTrack MapToVideoTextTrack(TextTrackInfo value)
        {
            return new VideoTextTrack
            {
                Default = value.Default,
                Kind = value.Kind,
                Label = value.Label,
                MimeType = value.MimeType,
                Src = value.Src,
                SrcLang = value.SrcLang
            };
        }

        private static IngestTextTrack MapToIngestTextTrack(TextTrackInfo value)
        {
            return new IngestTextTrack
            {
                Default = value.Default,
                Kind = value.Kind,
                Label = value.Label,
                Url = value.Src,
                SrcLang = value.SrcLang
            };
        }
    }
}