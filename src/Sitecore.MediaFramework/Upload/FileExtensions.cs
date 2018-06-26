namespace Sitecore.MediaFramework.Upload
{    
  using Newtonsoft.Json;

  public class FileExtensions
  {
    [JsonProperty("message")]
    public string Message { get; set; }
    [JsonProperty("ext")]
    public string Extensions { get; set; }
  }
}