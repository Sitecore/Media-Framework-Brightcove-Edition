
namespace Sitecore.MediaFramework.Data.Analytics
{
  using Sitecore.Analytics.Aggregation.Data.Model;

  public class MediaFrameworkEvents : Fact<MediaFrameworkEventsKey, MediaFrameworkEventsValue>
  {
    public MediaFrameworkEvents()
      : base(MediaFrameworkEventsValue.Reduce)
    {
    }
  }
}