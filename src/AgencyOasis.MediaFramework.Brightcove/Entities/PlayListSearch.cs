using System.Collections.Generic;
using AgencyOasis.MediaFramework.Brightcove.Json.Converters;
using Newtonsoft.Json;
using Sitecore.MediaFramework.Brightcove.Entities;

namespace AgencyOasis.MediaFramework.Brightcove.Entities
{
    public class PlayListSearch
    {
        public TagInclusion TagInclusion { get; set; }

        public List<string> FilterTags { get; set; }
    }
}
