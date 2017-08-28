using Newtonsoft.Json;

namespace Brightcove.MediaFramework.Brightcove.Entities
{
    public class IngestMaster
    {
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("use_archived_master", NullValueHandling = NullValueHandling.Ignore)]
        public bool? UseArchivedMaster { get; set; }
    }
}
