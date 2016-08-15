
namespace Sitecore.MediaFramework.Brightcove.Entities
{
  using System.Collections.Generic;

  using Newtonsoft.Json;

  /// <summary>
  /// Represents a playlist object from the Brightcove API
  /// For more information, see http://support.brightcove.com/en/video-cloud/docs/media-api-objects-reference#Playlist
  /// </summary>
  public class PlayList : Asset
  {
    /// <summary>
    /// Gets or sets the videoIds.
    /// </summary>
    [JsonProperty(PropertyName = "videoIds", NullValueHandling = NullValueHandling.Ignore)]
    public List<string> VideoIds { get; set; }

    /// <summary>
    /// Gets or sets the playlistType.
    /// </summary>
    [JsonProperty(PropertyName = "playlistType", NullValueHandling = NullValueHandling.Ignore)]
    public string PlaylistType { get; set; }

    /// <summary>
    /// Gets or sets the filterTags.
    /// </summary>
    [JsonProperty(PropertyName = "tagInclusionRule", NullValueHandling = NullValueHandling.Ignore)]
    public string TagInclusion { get; set; }

    /// <summary>
    /// Gets or sets the filterTags.
    /// </summary>
    [JsonProperty(PropertyName = "filterTags", NullValueHandling = NullValueHandling.Ignore)]
    public List<string> FilterTags { get; set; }
  }
}