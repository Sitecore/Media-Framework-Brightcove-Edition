// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResolveEventTrigger.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>                                                                                                        
//   Determines the event trigger.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Pipelines.Analytics
{
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Analytics;
  using Sitecore.MediaFramework.Diagnostics;

  /// <summary>
  /// Determines the event trigger.
  /// </summary>
  public class ResolveEventTrigger : PlayerEventProcessorBase
  {
    /// <summary>
    /// Processes determining the event trigger.
    /// </summary>
    /// <param name="args">                              
    /// The args.
    /// </param>
    public override void Process(PlayerEventArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.Properties, "args.Parameters");

      IEventTrigger trigger = MediaFrameworkContext.GetEventTrigger(args.Properties.Template);
      if (trigger != null)
      {
        args.Trigger = trigger;
      }
      else
      {
        LogHelper.Warn("Player event trigger couldn't be determine for the player.", this);

        args.AbortPipeline();
      }
    }
  }
}