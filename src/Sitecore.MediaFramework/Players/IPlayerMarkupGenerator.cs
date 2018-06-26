// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPlayerMarkupGenerator.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   The Player Markup Genarator interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Players
{
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup;

  /// <summary>
  /// The Player Markup Genarator interface.
  /// </summary>
  public interface IPlayerMarkupGenerator
  {
    /// <summary>
    /// Generates player markup.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    PlayerMarkupResult Generate(MediaGenerateMarkupArgs args);

    /// <summary>
    /// The get frame.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    string GetFrame(MediaGenerateMarkupArgs args);

    /// <summary>
    /// The get frame.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    string GetPreviewImage(MediaGenerateMarkupArgs args);    

    /// <summary>
    /// The get frame url.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    string GenerateFrameUrl(MediaGenerateMarkupArgs args);

    string GenerateLinkHtml(MediaGenerateMarkupArgs args);

    Item GetDefaultPlayer(MediaGenerateMarkupArgs args);

    string GetMediaId(Item item);
  }
}