namespace Sitecore.MediaFramework.Upload
{
  using System;

  using Newtonsoft.Json;

  public class AccountUploadStatus
  {
    public AccountUploadStatus()
    {
    }

    public AccountUploadStatus(Guid mediaItemId, Guid accountId, byte progress, string error, bool canceled)
    {
      this.MediaItemId = mediaItemId;
      this.AccountId = accountId;
      this.Progress = progress;
      this.Error = error;
      this.Canceled = canceled;
    }

    [JsonProperty("accountId")]                              
    public Guid AccountId { get; set; }

    [JsonProperty("progress")]
    public byte Progress { get; set; }

    [JsonProperty("error")]
    public string Error { get; set; }

    [JsonProperty("mediaItemId")]
    public Guid MediaItemId { get; set; }

    [JsonProperty("canceled")]
    public bool Canceled { get; set; }
  }
}