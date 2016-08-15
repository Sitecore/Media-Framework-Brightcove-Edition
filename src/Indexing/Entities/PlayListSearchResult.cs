namespace Sitecore.MediaFramework.Brightcove.Indexing.Entities
{
  using Sitecore.ContentSearch;
  using Sitecore.Data;

  /// <summary>
  /// PlayList Index
  /// </summary>
  public class PlaylistSearchResult : AssetSearchResult
  {
    /// <summary>
    /// Gets or sets the videoIds.
    /// </summary>
    [IndexField("videoids")]
    public ID[] VideoIds { get; set; }

    /// <summary>
    /// Gets or sets the playlistType.
    /// </summary>
    [IndexField("playlisttype")]
    public string PlaylistType { get; set; }

    /// <summary>
    /// Gets or sets the playlistType.
    /// </summary>
    [IndexField("taginclusion")]
    public string TagInclusion { get; set; }

    /// <summary>
    /// Gets or sets the filterTags.
    /// </summary>
    [IndexField("filtertags")]
    public ID[] FilterTags { get; set; }
  }
}