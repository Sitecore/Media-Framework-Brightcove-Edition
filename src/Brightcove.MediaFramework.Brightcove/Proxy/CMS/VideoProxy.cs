using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Brightcove.MediaFramework.Brightcove.Entities.Collections;
using Brightcove.MediaFramework.Brightcove.Entities.Response;
using RestSharp;
using Brightcove.MediaFramework.Brightcove.Entities;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Diagnostics;

namespace Brightcove.MediaFramework.Brightcove.Proxy.CMS
{
    public class VideoProxy : BaseProxy
    {
        public VideoProxy(IAuthenticator authenticator)
            : base(authenticator)
        {
        }

        public Entities.Collections.PagedCollection<Video> RetrieveList(int limit, int offset)
        {
            var videos = this.RetrieveList<Video>("read_videos", limit, offset);

            if (videos != null)
            {
                Log.Info("Video count: " + videos.ToList().Count, this);
            }

            return videos;
        }

        public Video Create(Video entity)
        {
            return this.Create<Video>("create_video", entity);
        }

        public Video Patch(Video entity)
        {
            var id = entity.Id;
            entity.Id = null;
            entity.CreationDate = null;
            entity.Images = null;
            entity.LastModifiedDate = null;
            entity.PublishedDate = null;
            entity.Duration = null;
            entity.Sharing = null;
            return this.Patch<Video>("update_video", entity, "video_id", id);
        }

        public bool Delete(string id)
        {
            return this.Delete("delete_video", "video_id", id);
        }

        public Video RetrieveById(string id)
        {
            return this.RetrieveById<Video>("read_video_by_id", "video_id", id);
        }

        public AssetRenditionCollection RetrieveSourcesById(string id)
        {
            return this.RetrieveById<AssetRenditionCollection>("read_video_sources_by_video_id", "video_id", id);
        }

        public CustomFieldsResponse RetrieveCustomFields()
        {
            return this.Retrieve<CustomFieldsResponse>("read_custom_fields");
        }

        public void UpdateTextTracks(string videoId, IList<VideoTextTrack> textTracks)
        {
            var video = new Video
            {
                Id = videoId,
                TextTracks = new Collection<VideoTextTrack>(textTracks)
            };

            this.Patch(video);
        }

        public void UpdateCustomFields(string videoId, IList<FieldInfo> customFields)
        {
            var video = new Video
            {
                Id = videoId,
                CustomFields = customFields.ToDictionary(i => i.Id, i => i.Value)
            };

            this.Patch(video);
        }

        protected override string ServiceName
        {
            get { return Constants.BrightCoveCmsService; }
        }
    }
}