using Sitecore.MediaFramework;

namespace AgencyOasis.MediaFramework.Brightcove.Analytics
{
  public class VideoEventTrigger : EventTrigger
  {
    public override void InitEvents()
    {
      this.AddEvent(Sitecore.MediaFramework.Brightcove.TemplateIDs.Video, PlaybackEvents.PlaybackStarted.ToString(), "Brightcove video is started.");
      this.AddEvent(Sitecore.MediaFramework.Brightcove.TemplateIDs.Video, PlaybackEvents.PlaybackCompleted.ToString(), "Brightcove video is completed.");
      this.AddEvent(Sitecore.MediaFramework.Brightcove.TemplateIDs.Video, PlaybackEvents.PlaybackChanged.ToString(), "Brightcove video progress is changed.");
      this.AddEvent(Sitecore.MediaFramework.Brightcove.TemplateIDs.Video, PlaybackEvents.PlaybackError.ToString(), "Brightcove video playback error.");
    }
  }
}
