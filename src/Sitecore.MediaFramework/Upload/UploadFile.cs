namespace Sitecore.MediaFramework.Upload
{
  using System;

  using Newtonsoft.Json;

  public class UploadingFile
  {
    [JsonProperty("error")]
    public string Error { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("size")]
    public int Size { get; set; }
    [JsonProperty("thumbnailUrl")]
    public string ThumbnailUrl { get; set; }
    [JsonProperty("id")]
    public Guid ID { get; set; }
  }
}