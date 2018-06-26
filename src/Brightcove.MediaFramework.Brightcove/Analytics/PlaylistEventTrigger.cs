using Sitecore.MediaFramework;

namespace Brightcove.MediaFramework.Brightcove.Analytics
{
  public class PlaylistEventTrigger : EventTrigger
  {
    public override void InitEvents()
    {
      this.AddEvent(TemplateIDs.Playlist, PlaybackEvents.PlaybackStarted.ToString(), "Brightcove video is started.");
      this.AddEvent(TemplateIDs.Playlist, PlaybackEvents.PlaybackCompleted.ToString(), "Brightcove video is completed.");
      this.AddEvent(TemplateIDs.Playlist, PlaybackEvents.PlaybackChanged.ToString(), "Brightcove video progress is changed.");
      this.AddEvent(TemplateIDs.Playlist, PlaybackEvents.PlaybackError.ToString(), "Brightcove video playback error.");
    }
  }
}
