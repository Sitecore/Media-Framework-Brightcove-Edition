namespace Sitecore.MediaFramework.Brightcove.Entities
{
  using Newtonsoft.Json;

  public class VideoToUpload
  {
    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    [JsonProperty("shortDescription", NullValueHandling = NullValueHandling.Ignore)]
    public string ShortDescription { get; set; }

    [JsonProperty("startDate")]
    public string StartDate { get; set; }

    [JsonProperty("endDate")]
    public string EndDate { get; set; }
  }
}