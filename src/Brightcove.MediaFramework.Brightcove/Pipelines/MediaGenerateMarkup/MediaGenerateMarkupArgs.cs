// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaGenerateMarkupArgs.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   The media generate markup args.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Brightcove.MediaFramework.Brightcove.Players;
using Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup;

namespace Brightcove.MediaFramework.Brightcove.Pipelines.MediaGenerateMarkup
{
  using System.Collections.Generic;

  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Players;
  using Sitecore.Pipelines;

  /// <summary>
  /// The media generate markup args.
  /// </summary>
  public class MediaGenerateMarkupArgszzz : PipelineArgs
  {
    public MediaGenerateMarkupArgszzz()
    {
      this.Result = new PlayerMarkupResult();
      this.Properties = new BrightcovePlayerProperties();
      this.PlaybackEvents = new Dictionary<string, List<string>>();
    }

    public MarkupType MarkupType { get; set; }

    /// <summary>
    /// Gets or sets the generator.
    /// </summary>
    public IPlayerMarkupGenerator Generator { get; set; }

    /// <summary>
    /// Gets or sets the media source item.
    /// </summary>
    public BrightcovePlayerProperties Properties { get; set; }

    public Database Database { get; set; }

    public Item MediaItem { get; set; }

    public Item AccountItem { get; set; }

    public Item PlayerItem { get; set; }

    public string LinkTitle { get; set; }

    public Dictionary<string, List<string>> PlaybackEvents { get; set; }
    /// <summary>
    /// Gets or sets the markup.
    /// </summary>
    public PlayerMarkupResult Result { get; set; }
  }
}