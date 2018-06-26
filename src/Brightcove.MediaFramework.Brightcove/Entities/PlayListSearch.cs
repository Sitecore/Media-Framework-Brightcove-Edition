using System.Collections.Generic;
using Brightcove.MediaFramework.Brightcove.Json.Converters;
using Newtonsoft.Json;

namespace Brightcove.MediaFramework.Brightcove.Entities
{
    public class PlayListSearch
    {
        public TagInclusion TagInclusion { get; set; }

        public List<string> FilterTags { get; set; }
    }
}
