namespace Sitecore.MediaFramework.Brightcove.Entities
{
  using System;
  using System.Collections.Generic;
  using Newtonsoft.Json;
  using Newtonsoft.Json.Converters;

  using Sitecore.MediaFramework.Brightcove.Json.Converters;

  /// <summary>
  /// Represents a video object from the Brightcove API
  /// For more information, see http://support.brightcove.com/en/video-cloud/docs/media-api-objects-reference#Video
  /// </summary>
  public class Video : Asset
  {
    /// <summary>
    /// Gets or sets the creationDate.
    /// </summary>
    [JsonProperty("creationDate", NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(BrightcoveDateConverter))]
    public DateTime? CreationDate { get; set; }

    /// <summary>
    /// Gets or sets the longDescription.
    /// </summary>
    [JsonProperty("longDescription", NullValueHandling = NullValueHandling.Ignore)]
    public string LongDescription { get; set; }

    /// <summary>
    /// Gets or sets the publishedDate.
    /// </summary>
    [JsonProperty("publishedDate", NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(BrightcoveDateConverter))]
    public DateTime? PublishedDate { get; set; }

    /// <summary>
    /// Gets or sets the lastModifiedDate.
    /// </summary>
    [JsonProperty("lastModifiedDate", NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(BrightcoveDateConverter))]
    public DateTime? LastModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets the economics.
    /// </summary>
    [JsonProperty("economics", NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(StringEnumConverter))]
    public Economics? Economics { get; set; }

    /// <summary>
    /// Gets or sets the linkURL.
    /// </summary>
    [JsonProperty("linkURL", NullValueHandling = NullValueHandling.Ignore)]
    public string LinkURL { get; set; }

    /// <summary>
    /// Gets or sets the linkText.
    /// </summary>
    [JsonProperty("linkText", NullValueHandling = NullValueHandling.Ignore)]
    public string LinkText { get; set; }

    /// <summary>
    /// Gets or sets the tags.
    /// </summary>
    [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore)]
    public List<string> Tags { get; set; }

    /// <summary>
    /// Gets or sets the videoStillURL.
    /// </summary>
    [JsonProperty("videoStillURL", NullValueHandling = NullValueHandling.Ignore)]
    public string VideoStillURL { get; set; }

    /// <summary>
    /// Gets or sets the length.
    /// </summary>
    [JsonProperty("length", NullValueHandling = NullValueHandling.Ignore)]
    public long? Length { get; set; }

    /// <summary>
    /// Gets or sets the playsTotal.
    /// </summary>
    [JsonProperty("playsTotal", NullValueHandling = NullValueHandling.Ignore)]
    public int? PlaysTotal { get; set; }

    /// <summary>
    /// Gets or sets the playsTrailingWeek.
    /// </summary>
    [JsonProperty("playsTrailingWeek", NullValueHandling = NullValueHandling.Ignore)]
    public int? PlaysTrailingWeek { get; set; }

    [JsonProperty("itemState", NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(StringEnumConverter))]
    public ItemState? ItemState { get; set; }

    [JsonProperty("customFields", NullValueHandling = NullValueHandling.Ignore)]
    public Dictionary<string, string> CustomFields { get; set; }
  }
}