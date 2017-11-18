using Sitecore.ContentSearch;
using Sitecore.MediaFramework.Entities;

namespace Brightcove.MediaFramework.Brightcove.Indexing.Entities
{
    public class AssetSearchResult : MediaServiceSearchResult
    {
        [IndexField("id")]
        public string Id { get; set; }

        [IndexField("name")]
        public string AssetName { get; set; }

        [IndexField("referenceid")]
        public string ReferenceId { get; set; }

        [IndexField("thumbnailurl")]
        public string ThumbnailUrl { get; set; }

        [IndexField("shortdescription")]
        public string ShortDescription { get; set; }
    }
}
