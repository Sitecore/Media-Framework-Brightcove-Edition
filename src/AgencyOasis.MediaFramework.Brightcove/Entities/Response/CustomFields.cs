using System.Collections.ObjectModel;
using AgencyOasis.MediaFramework.Brightcove.Entities.Collections;
using Newtonsoft.Json;

namespace AgencyOasis.MediaFramework.Brightcove.Entities.Response
{
    public class CustomFieldsResponse
    {
        [JsonProperty("custom_fields", NullValueHandling = NullValueHandling.Ignore)]
        public FieldCollection CustomFields { get; set; }

        [JsonProperty("max_custom_fields", NullValueHandling = NullValueHandling.Ignore)]
        public string MaxCustomFields { get; set; }

        [JsonProperty("standard_fields", NullValueHandling = NullValueHandling.Ignore)]
        public FieldCollection StandardFields { get; set; }
    }
}