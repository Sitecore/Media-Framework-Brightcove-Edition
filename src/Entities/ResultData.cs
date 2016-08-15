namespace Sitecore.MediaFramework.Brightcove.Entities
{
  using Newtonsoft.Json;

  public class ResultData<T>
  {
    [JsonProperty(PropertyName = "result", NullValueHandling = NullValueHandling.Ignore)]
    public T Result { get; set; }

    [JsonProperty(PropertyName = "error", NullValueHandling = NullValueHandling.Ignore)]
    public RpcError Error { get; set; }

    [JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    public class RpcError
    {
      [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
      public string Name { get; set; }
      
      [JsonProperty(PropertyName = "message", NullValueHandling = NullValueHandling.Ignore)]
      public string Message { get; set; }

      [JsonProperty(PropertyName = "code", NullValueHandling = NullValueHandling.Ignore)]
      public int Code { get; set; }
    }
  }
}