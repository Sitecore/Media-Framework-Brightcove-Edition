using Newtonsoft.Json;

namespace AgencyOasis.MediaFramework.Brightcove.Entities
{
    public class VideoLink
    {
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string URL { get; set; }
    }
}
