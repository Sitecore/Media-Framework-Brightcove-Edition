using System.Collections.Generic;
using Brightcove.MediaFramework.Brightcove.Json.Converters;
using Newtonsoft.Json;
using System;

namespace Brightcove.MediaFramework.Brightcove.Entities
{
    /// <summary>
    /// Represents a playlist object from the Brightcove API
    /// For more information, see http://support.brightcove.com/en/video-cloud/docs/media-api-objects-reference#Playlist
    /// </summary>
    public class PlayList : Asset
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "video_ids")]
        public List<string> VideoIds { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type")]
        public string PlaylistType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "search")]
        [JsonConverter(typeof(BrightcovePlaylistSearchFieldConverter))]
        public PlayListSearch PlayListSearch { get; set; }

        [JsonProperty("favorite", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Favorite { get; set; }

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CreationDate { get; set; }

        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? LastModifiedDate { get; set; }
    }
}