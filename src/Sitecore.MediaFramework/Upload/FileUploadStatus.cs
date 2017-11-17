namespace Sitecore.MediaFramework.Upload
{
  using System;
  using System.Collections.Generic;

  using Newtonsoft.Json;

  public class FileUploadStatus
  {
    public FileUploadStatus()
    {                                
      this.ProgressList = new List<AccountUploadStatus>();
    }

    public FileUploadStatus(Guid fileId) : this()
    {
      this.FileId = fileId;
    }

    [JsonProperty("fileId")]
    public Guid FileId { get; set; }

    [JsonProperty("progressList")]
    public List<AccountUploadStatus> ProgressList { get; set; }      
  }
}