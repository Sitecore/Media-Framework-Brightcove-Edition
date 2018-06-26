// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaGenerateMarkupPipeline.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   The media generate markup pipeline.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup
{
  using Sitecore.Diagnostics;
  using Sitecore.Pipelines;

  /// <summary>
  /// The media generate markup pipeline.
  /// </summary>
  public class MediaGenerateMarkupPipeline
  {
    /// <summary>
    /// Runs the pipeline.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public static void Run(MediaGenerateMarkupArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      CorePipeline.Run("mediaFramework.mediaGenerateMarkup", args);
    }
  }
}