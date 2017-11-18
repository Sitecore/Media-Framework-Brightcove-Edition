using System;
using System.Collections.Generic;
using Sitecore.ContentSearch;
using Sitecore.Data;

namespace Brightcove.MediaFramework.Brightcove.Indexing.Entities
{
    public class PlaylistSearchResult : AssetSearchResult
    {
        [IndexField("videoids")]
        public ID[] VideoIds { get; set; }

        [IndexField("playlisttype")]
        public string PlaylistType { get; set; }

        [IndexField("taginclusion")]
        public string TagInclusion { get; set; }

        [IndexField("filtertags")]
        public ID[] FilterTags { get; set; }
    }
}
