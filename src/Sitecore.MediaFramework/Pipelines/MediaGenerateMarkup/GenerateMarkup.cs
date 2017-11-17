// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenerateMarkup.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   Generates a markup.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup
{
  using Sitecore.Diagnostics;
  using Sitecore.Globalization;

  /// <summary>
  /// Generates a markup.
  /// </summary>
  public class GenerateMarkup : MediaGenerateMarkupProcessorBase
  {
    /// <summary>
    /// Processes generating of a markup.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public override void Process(MediaGenerateMarkupArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.Generator, "args.Generator");

      switch (args.MarkupType)
      {
        case MarkupType.Frame:
          args.Result.Html = args.Generator.GetFrame(args);
          break;
        case MarkupType.FrameUrl:
          args.Result.Html = args.Generator.GenerateFrameUrl(args);
          break;
        case MarkupType.Link:
          if (string.IsNullOrEmpty(args.LinkTitle))
          {
            args.LinkTitle = Translate.Text("Media Link");
          }

          args.Result.Html = args.Generator.GenerateLinkHtml(args);
          break;
        case MarkupType.Html:
          if (!args.Properties.ForceRender && Context.PageMode.IsExperienceEditorEditing)
          {
            args.Result.Html = args.Generator.GetPreviewImage(args);
          }
          else
          {
            args.Result = args.Generator.Generate(args);
          }
          break;
      }
    }
  }
}