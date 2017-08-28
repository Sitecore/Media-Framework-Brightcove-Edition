
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Brightcove.MediaFramework.Brightcove.Entities
{
    public class FieldInfo
    {
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("display_name", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; }

        [JsonProperty("enum_values", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> EnumValues { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("required", NullValueHandling = NullValueHandling.Ignore)]
        public bool Required { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        public string Value { get; set; }
    }
}