namespace Sitecore.MediaFramework.Brightcove.Indexing.Entities
{
  using Sitecore.ContentSearch;
  using Sitecore.MediaFramework.Entities;

  public class AssetSearchResult : MediaServiceSearchResult
  {
    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    [IndexField("id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    [IndexField("name")]
    public string AssetName { get; set; }

    /// <summary>
    /// Gets or sets the referenceId.
    /// </summary>
    [IndexField("referenceid")]
    public string ReferenceId { get; set; }

    /// <summary>
    /// Gets or sets the thumbnailURL.
    /// </summary>
    [IndexField("thumbnailurl")]
    public string ThumbnailUrl { get; set; }

    /// <summary>
    /// Gets or sets the shortDescription.
    /// </summary>
    [IndexField("shortdescription")]
    public string ShortDescription { get; set; }

    //[IgnoreIndexField]
    //public override string EntityId { get { return this.Id; } }
  }
}