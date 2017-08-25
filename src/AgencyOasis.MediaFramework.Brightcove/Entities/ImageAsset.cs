using System.Collections.Generic;
using Newtonsoft.Json;

namespace AgencyOasis.MediaFramework.Brightcove.Entities
{
    public class ImageAsset
    {
        [JsonProperty("asset_id", NullValueHandling = NullValueHandling.Ignore)]
        public string AssetId { get; set; }

        [JsonProperty("src", NullValueHandling = NullValueHandling.Ignore)]
        public string Src { get; set; }

        [JsonProperty("sources", NullValueHandling = NullValueHandling.Ignore)]
        public IList<AssetSource> Sources { get; set; }
    }
}
