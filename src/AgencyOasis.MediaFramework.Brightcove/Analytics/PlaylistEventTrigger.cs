using Sitecore.MediaFramework;

namespace AgencyOasis.MediaFramework.Brightcove.Analytics
{
  public class PlaylistEventTrigger : EventTrigger
  {
    public override void InitEvents()
    {
      this.AddEvent(Sitecore.MediaFramework.Brightcove.TemplateIDs.Playlist, PlaybackEvents.PlaybackStarted.ToString(), "Brightcove video is started.");
      this.AddEvent(Sitecore.MediaFramework.Brightcove.TemplateIDs.Playlist, PlaybackEvents.PlaybackCompleted.ToString(), "Brightcove video is completed.");
      this.AddEvent(Sitecore.MediaFramework.Brightcove.TemplateIDs.Playlist, PlaybackEvents.PlaybackChanged.ToString(), "Brightcove video progress is changed.");
      this.AddEvent(Sitecore.MediaFramework.Brightcove.TemplateIDs.Playlist, PlaybackEvents.PlaybackError.ToString(), "Brightcove video playback error.");
    }
  }
}
