using Newtonsoft.Json;

namespace AgencyOasis.MediaFramework.Brightcove.Entities
{
    public class AssetSource
    {
        [JsonProperty("src", NullValueHandling = NullValueHandling.Ignore)]
        public string Src { get; set; }
    }
}
