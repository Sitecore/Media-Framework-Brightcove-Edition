// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerEventPipeline.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   The player event pipeline.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Pipelines.Analytics
{
  using Sitecore.Diagnostics;
  using Sitecore.Pipelines;

  /// <summary>
  /// The player event pipeline.
  /// </summary>
  public class PlayerEventPipeline
  {
    /// <summary>
    /// Runs the pipeline.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public static void Run(PlayerEventArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      CorePipeline.Run("mediaFramework.playerEvent", args);
    }
  }
}