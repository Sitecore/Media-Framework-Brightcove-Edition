
using Newtonsoft.Json;

namespace Brightcove.MediaFramework.Brightcove.Entities
{
    public class Notification
    {
        [JsonProperty("entity", NullValueHandling = NullValueHandling.Ignore)]
        public string Entity { get; set; }

        [JsonProperty("entityType", NullValueHandling = NullValueHandling.Ignore)]
        public string EntityType { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }

        [JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
        public string Action { get; set; }

        public string ToString()
        {
            return string.Format("brightcove notification - (entity:{0},entityType:{1},status:{2},version:{3},action:{4})", Entity, EntityType, Status, Version, Action);
        }
    }
}
