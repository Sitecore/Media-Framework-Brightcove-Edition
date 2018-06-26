// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DetermineMarkupGenerator.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   Determines the markup generator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup
{
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Diagnostics;
  using Sitecore.MediaFramework.Players;

  /// <summary>
  /// Determines the markup generator.
  /// </summary>
  public class ResolveMarkupGenerator : MediaGenerateMarkupProcessorBase
  {
    /// <summary>
    /// Processes determining the markup generator.
    /// </summary>
    /// <param name="args">                              
    /// The args.
    /// </param>
    public override void Process(MediaGenerateMarkupArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.MediaItem, "args.MediaItem");

      IPlayerMarkupGenerator generator = MediaFrameworkContext.GetPlayerMarkupGenerator(args.MediaItem.TemplateID);
      if (generator != null)
      {
        args.Generator = generator;

        if (args.MarkupType == MarkupType.Html)
        {
          args.Properties.MediaId = args.Generator.GetMediaId(args.MediaItem);
        }
      }
      else
      {
        LogHelper.Warn("Player markup generator couldn't be determine for player. Player markup generation will be stopped.", this);

        args.AbortPipeline();
      }
    }
  }
}