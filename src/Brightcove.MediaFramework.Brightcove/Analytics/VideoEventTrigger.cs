using Sitecore.MediaFramework;

// --------------------------------------------------------------------------------------------------------------------
// <summary>
//   The player event trigger.
// </summary>                                                                                
// --------------------------------------------------------------------------------------------------------------------

namespace Brightcove.MediaFramework.Brightcove.Analytics
{
  public class VideoEventTrigger : EventTrigger
  {
    public override void InitEvents()
    {
      this.AddEvent(TemplateIDs.Video, PlaybackEvents.PlaybackStarted.ToString(), "Brightcove video is started.");
      this.AddEvent(TemplateIDs.Video, PlaybackEvents.PlaybackCompleted.ToString(), "Brightcove video is completed.");
      this.AddEvent(TemplateIDs.Video, PlaybackEvents.PlaybackChanged.ToString(), "Brightcove video progress is changed.");
      this.AddEvent(TemplateIDs.Video, PlaybackEvents.PlaybackError.ToString(), "Brightcove video playback error.");
    }
  }
}
