
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Brightcove.MediaFramework.Brightcove.Entities
{
    public class VideoTextTrack : TextTrack
    {
        [JsonProperty("src", NullValueHandling = NullValueHandling.Ignore)]
        public string Src { get; set; }

        [JsonProperty("mime_type", NullValueHandling = NullValueHandling.Ignore)]
        public string MimeType { get; set; }
    }
}