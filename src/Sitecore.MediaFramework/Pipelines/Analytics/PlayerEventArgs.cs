// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerEventArgs.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   The player event args.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Pipelines.Analytics
{
  using Sitecore.MediaFramework.Analytics;
  using Sitecore.MediaFramework.Players;
  using Sitecore.Pipelines;

  /// <summary>
  /// The player event args.
  /// </summary>
  public class PlayerEventArgs : PipelineArgs
  {
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string EventName { get; set; }

    /// <summary>
    /// Gets or sets the parameters.
    /// </summary>
    public EventProperties Properties { get; set; }

    /// <summary>
    /// Gets or sets the trigger.
    /// </summary>
    public IEventTrigger Trigger { get; set; }
  }
}