namespace Sitecore.MediaFramework.Brightcove.Entities.Collections
{
  using System.Collections.Generic;

  using Newtonsoft.Json;

  public class PagedCollection<T>
  {
    [JsonProperty("items")]
    public List<T> Items { get; set; }

    [JsonProperty("page_number")]
    public int PageNumber { get; set; }

    [JsonProperty("page_size")]
    public int PageSize { get; set; }

    [JsonProperty("total_count")]
    public int TotalCount { get; set; }
  }
}