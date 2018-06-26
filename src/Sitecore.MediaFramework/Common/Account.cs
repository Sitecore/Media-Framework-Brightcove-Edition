namespace Sitecore.MediaFramework.Common
{
  using System;
  using System.Collections.Generic;

  using Newtonsoft.Json;

  using Sitecore.Data.Items;

  public class Account
  {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("id")]
    public Guid ID { get; set; }
    
    [JsonProperty("accountTemplateId")]
    public Guid AccountTemplateId { get; set; }
    
    [JsonProperty("selected")]
    public bool Selected { get; set; }
    
    [JsonProperty("exts")]
    public List<string> Extensions { get; set; }
    
    [JsonProperty("supportCancel")]
    public bool SupportCancel { get; set; }     

    public Account()
    {
    }

    public Account(Item accountItem, bool selected)
    {
      this.ID = accountItem.ID.Guid;
      this.Name = accountItem.Name;
      this.AccountTemplateId = accountItem.TemplateID.Guid;
      
      this.Selected = selected;

      var uploadExecuter = MediaFrameworkContext.GetUploadExecuter(accountItem);
      if (uploadExecuter != null)
      {
        this.Extensions = new List<string>(uploadExecuter.FileExtensions);
        this.SupportCancel = uploadExecuter.SupportCanceling;
      }
      else
      {
        this.Extensions = new List<string>();
        this.SupportCancel = true;
      }
    }      
  }
}