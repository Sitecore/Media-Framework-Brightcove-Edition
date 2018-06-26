// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TriggerDMSEvent.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   Triggers an player's DMS-event.
// </summary>                                                          
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Pipelines.Analytics
{
  using Sitecore.Diagnostics;

  /// <summary>
  /// Triggers an player's DMS-event.
  /// </summary>
  public class TriggerDMSEvent : PlayerEventProcessorBase
  {
    /// <summary>
    /// Processes triggering of an DMS-event.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public override void Process(PlayerEventArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.EventName, "args.Name");
      Assert.ArgumentNotNull(args.Properties, "args.Parameters");

      args.Trigger.Register(args);
    }
  }
}