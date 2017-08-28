using Newtonsoft.Json;

namespace Brightcove.MediaFramework.Brightcove.Entities
{
    public class VideoSharing
    {
        [JsonProperty("by_external_acct", NullValueHandling = NullValueHandling.Ignore)]
        public bool ByExternalAcct { get; set; }

        [JsonProperty("by_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ById { get; set; }

        [JsonProperty("source_id", NullValueHandling = NullValueHandling.Ignore)]
        public string SourceId { get; set; }

        [JsonProperty("to_external_acct", NullValueHandling = NullValueHandling.Ignore)]
        public bool ToExternalAcct { get; set; }

        [JsonProperty("by_reference", NullValueHandling = NullValueHandling.Ignore)]
        public bool ByReference { get; set; }
    }
}
