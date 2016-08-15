namespace Sitecore.MediaFramework.Brightcove.Entities
{
  using System.Collections.Generic;

  using Newtonsoft.Json;

  using Sitecore.MediaFramework.Brightcove.Security;

  public class PostData
  {
    public PostData(string method, BrightcoveAthenticator authenticator,string propertyName, object propertyValue)
      : this(method, authenticator, new Dictionary<string, object>{{propertyName,propertyValue}})
    {
    }

    public PostData(string method, BrightcoveAthenticator authenticator, Dictionary<string, object> parameters = null)
    {
      this.Method = method;
      this.Parameters = parameters ?? new Dictionary<string, object>();
      this.Parameters.Add("token",authenticator.WriteToken);
    }

    [JsonProperty(PropertyName = "method")]
    public string Method { get; set; }

    [JsonProperty(PropertyName = "params")]
    public Dictionary<string,object> Parameters { get; set; }
  }
}