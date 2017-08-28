
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Brightcove.MediaFramework.Brightcove.Entities
{
    public class IngestTextTrack : TextTrack
    {
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
    }
}