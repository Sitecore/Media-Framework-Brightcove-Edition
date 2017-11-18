using Sitecore.ContentSearch;
using Sitecore.MediaFramework.Entities;

namespace Brightcove.MediaFramework.Brightcove.Indexing.Entities
{
    public class TagSearchResult : MediaServiceSearchResult
    {
        [IndexField("name")]
        public string TagName { get; set; }
    }

}
