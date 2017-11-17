// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerEventProcessorBase.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   The player event base processor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Pipelines.Analytics
{
  /// <summary>
  /// The player event base processor.
  /// </summary>
  public abstract class PlayerEventProcessorBase
  {
    /// <summary>
    /// Processes player events.
    /// </summary>                                                       
    /// <param name="args">
    /// The args.
    /// </param>
    public abstract void Process(PlayerEventArgs args);
  }
}