// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerEventTrigger.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   The player event trigger.
// </summary>                                                                                
// --------------------------------------------------------------------------------------------------------------------
                                                                                         
namespace Sitecore.MediaFramework.Brightcove.Analytics
{
  using Sitecore.MediaFramework.Analytics;

  /// <summary>
  /// The player event trigger.
  /// </summary>
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