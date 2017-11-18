using System.ComponentModel;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Converters;
using Sitecore.Data;


namespace Brightcove.MediaFramework.Brightcove.Indexing.Entities
{
  public class VideoSearchResult : AssetSearchResult
  {
    [IndexField("creationdate")]
    public string CreationDate { get; set; }

    [IndexField("longdescription")]
    public string LongDescription { get; set; }

    [IndexField("publisheddate")]
    public string PublishedDate { get; set; }

    [IndexField("lastmodifieddate")]
    public string LastModifiedDate { get; set; }

    [IndexField("economics")]
    public string Economics { get; set; }

    [IndexField("linkurl")]
    public string LinkURL { get; set; }

    [IndexField("linktext")]
    public string LinkText { get; set; }

    [TypeConverter(typeof (IndexFieldEnumerableConverter))]
    [IndexField("tags")]
    public ID[] Tags { get; set; }

    [IndexField("videostillurl")]
    public string VideoStillURL { get; set; }

    [IndexField("customfields")]
    public string CustomFields { get; set; }
  }
}
