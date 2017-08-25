using Newtonsoft.Json;

namespace AgencyOasis.MediaFramework.Brightcove.Entities
{
    public class AssetRendition
    {
        [JsonProperty("app_name", NullValueHandling = NullValueHandling.Ignore)]
        public string AppName { get; set; }
    }
}
