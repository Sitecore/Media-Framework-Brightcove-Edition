namespace Sitecore.MediaFramework.Brightcove.Indexing.Entities
{
  using System.ComponentModel;
  using Sitecore.ContentSearch;
  using Sitecore.ContentSearch.Converters;
  using Sitecore.Data;

  public class VideoSearchResult : AssetSearchResult
  {
    /// <summary>
    /// Gets or sets the creationDate.
    /// </summary>
    [IndexField("creationdate")]
    public string CreationDate { get; set; }

    /// <summary>
    /// Gets or sets the longDescription.
    /// </summary>
    [IndexField("longdescription")]
    public string LongDescription { get; set; }

    /// <summary>
    /// Gets or sets the publishedDate.
    /// </summary>
    [IndexField("publisheddate")]
    public string PublishedDate { get; set; }

    /// <summary>
    /// Gets or sets the lastModifiedDate.
    /// </summary>
    [IndexField("lastmodifieddate")]
    public string LastModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets the economics.
    /// </summary>
    [IndexField("economics")]
    public string Economics { get; set; }

    /// <summary>
    /// Gets or sets the linkURL.
    /// </summary>
    [IndexField("linkurl")]
    public string LinkURL { get; set; }

    /// <summary>
    /// Gets or sets the linkText.
    /// </summary>
    [IndexField("linktext")]
    public string LinkText { get; set; }

    /// <summary>
    /// Gets or sets the tags.
    /// </summary>
    [IndexField("tags")]
    [TypeConverter(typeof(IndexFieldEnumerableConverter))]
    public ID[] Tags { get; set; }

    /// <summary>
    /// Gets or sets the videoStillURL.
    /// </summary>
    [IndexField("videostillurl")]
    public string VideoStillURL { get; set; }

    /// <summary>
    /// Gets or sets the length.
    /// </summary>
    [IndexField("length")]
    public string Length { get; set; }

    /// <summary>
    /// Gets or sets the playsTotal.
    /// </summary>
    [IndexField("playstotal")]
    public string PlaysTotal { get; set; }

    /// <summary>
    /// Gets or sets the playsTrailingWeek.
    /// </summary>
    [IndexField("playstrailingweek")]
    public string PlaysTrailingWeek { get; set; }

    [IndexField("customfields")]
    public string CustomFields { get; set; }
  }
}