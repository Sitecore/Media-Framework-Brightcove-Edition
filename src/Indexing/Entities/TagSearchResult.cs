namespace Sitecore.MediaFramework.Brightcove.Indexing.Entities
{
  using Sitecore.ContentSearch;
  using Sitecore.MediaFramework.Entities;

  public class TagSearchResult : MediaServiceSearchResult
  {
    [IndexField("name")]
    public string TagName { get; set; }

    //[IgnoreIndexField]
    //public override string EntityId { get { return this.TagName; } }
  }
}