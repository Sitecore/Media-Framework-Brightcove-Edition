namespace Sitecore.MediaFramework.Data.Analytics
{
  using System;

  using Sitecore.Analytics.Aggregation.Data.Model;
  using Sitecore.Analytics.Core;

  public class MediaFrameworkEventsKey : DictionaryKey
  {
    private string eventParameter = string.Empty;

    public DateTime Date { get; set; }

    public Hash128 MediaId { get; set; }

    public Guid PageEventDefinitionId { get; set; }

    public int SiteNameId { get; set; }

    public string EventParameter
    {
      get
      {
        return eventParameter ?? string.Empty;
      }
      set
      {
        eventParameter = value;
      }
    }
  }
}