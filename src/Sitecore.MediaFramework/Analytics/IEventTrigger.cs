namespace Sitecore.MediaFramework.Analytics
{
  using Sitecore.MediaFramework.Pipelines.Analytics;

  public interface IEventTrigger
  {
    void Register(PlayerEventArgs args);

    void InitEvents();
  }
}