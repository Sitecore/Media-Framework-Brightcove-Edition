namespace Sitecore.MediaFramework.Brightcove.Entities
{
  using Newtonsoft.Json;

  public class Asset
  {
    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the referenceId.
    /// </summary>
    [JsonProperty("referenceId", NullValueHandling = NullValueHandling.Ignore)]
    public string ReferenceId { get; set; }

    /// <summary>
    /// Gets or sets the thumbnailURL.
    /// </summary>
    [JsonProperty("thumbnailURL", NullValueHandling = NullValueHandling.Ignore)]
    public string ThumbnailUrl { get; set; }

    /// <summary>
    /// Gets or sets the shortDescription.
    /// </summary>
    [JsonProperty("shortDescription", NullValueHandling = NullValueHandling.Ignore)]
    public string ShortDescription { get; set; }

    public override string ToString()
    {
      return string.Format("(type:{0},id:{1},name:{2})", this.GetType().Name, this.Id, this.Name);
    }
  }
}