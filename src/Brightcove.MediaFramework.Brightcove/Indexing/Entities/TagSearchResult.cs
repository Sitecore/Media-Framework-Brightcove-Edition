using Sitecore.ContentSearch;
using Sitecore.MediaFramework.Entities;

namespace Brightcove.MediaFramework.Brightcove.Indexing.Entities
{
    public class TagSearchResult : AssetSearchResult
    {
        [IndexField("name")]
        public string TagName { get; set; }
    }

}
