namespace Sitecore.MediaFramework.Data.Analytics
{
  using Sitecore.Analytics.Aggregation.Data.Model;

  public class MediaFrameworkEventsValue : DictionaryValue
  {
    public long Count { get; set; }

    public static MediaFrameworkEventsValue Reduce(MediaFrameworkEventsValue left, MediaFrameworkEventsValue right)
    {
      return new MediaFrameworkEventsValue { Count = left.Count + right.Count };
    }
  }
}