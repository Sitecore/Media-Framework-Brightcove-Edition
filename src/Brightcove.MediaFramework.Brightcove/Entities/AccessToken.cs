using Newtonsoft.Json;

namespace Brightcove.MediaFramework.Brightcove.Entities
{
    public class AccessToken
  {
    [JsonProperty("access_token", NullValueHandling = NullValueHandling.Ignore)]
    public string Token { get; set; }

    [JsonProperty("token_type", NullValueHandling = NullValueHandling.Ignore)]
    public string TokenType { get; set; }

    [JsonProperty("expires_in", NullValueHandling = NullValueHandling.Ignore)]
    public int ExpiresInSeconnds { get; set; }
  }
}
