using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Brightcove.MediaFramework.Brightcove.Entities
{
    public class IngestVideo
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("master", NullValueHandling = NullValueHandling.Ignore)]
        public IngestMaster IngestMaster { get; set; }

        [JsonProperty("profile", NullValueHandling = NullValueHandling.Ignore)]
        public string Profile { get; set; }

        [JsonProperty("capture-images", NullValueHandling = NullValueHandling.Ignore)]
        public bool? CaptureImages { get; set; }

        [JsonProperty("callbacks", NullValueHandling = NullValueHandling.Ignore)]
        public Collection<string> Callbacks { get; set; }

        [JsonProperty("text_tracks", NullValueHandling = NullValueHandling.Ignore)]
        public Collection<IngestTextTrack> TextTracks { get; set; }

        [JsonIgnore]
        public string VideoId { get; set; }
    }
}