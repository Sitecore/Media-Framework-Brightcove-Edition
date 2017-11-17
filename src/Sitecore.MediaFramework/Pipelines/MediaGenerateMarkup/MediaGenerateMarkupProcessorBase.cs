// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaGenerateMarkupProcessorBase.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   The media generate markup base processor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup
{
  /// <summary>
  /// The media generate markup base processor.
  /// </summary>
  public abstract class MediaGenerateMarkupProcessorBase
  {
    /// <summary>
    /// Processes media markup generation.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public abstract void Process(MediaGenerateMarkupArgs args);
  }
}