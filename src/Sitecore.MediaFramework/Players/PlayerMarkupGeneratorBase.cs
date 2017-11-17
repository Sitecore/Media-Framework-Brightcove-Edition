namespace Sitecore.MediaFramework.Players
{
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup;
  using Sitecore.Text;

  public abstract class PlayerMarkupGeneratorBase : IPlayerMarkupGenerator
  {
    public string AnalyticsScriptUrl { get; set; }

    public abstract PlayerMarkupResult Generate(MediaGenerateMarkupArgs args);

    public abstract string GetPreviewImage(MediaGenerateMarkupArgs args);

    public virtual string GetFrame(MediaGenerateMarkupArgs args)
    {
      return
        string.Format(
          "<iframe scrolling='no' class='player-frame' width='{0}' height='{1}' frameborder='0' src='{2}'></iframe>",
          args.Properties.Width,
          args.Properties.Height,
          this.GenerateFrameUrl(args));
    }

    public virtual string GenerateFrameUrl(MediaGenerateMarkupArgs args)
    {
      var url = new UrlString(Constants.PlayerIframeUrl);
      foreach (string key in args.Properties.Collection)
      {
        url[key] = args.Properties.Collection[key];
      }

      return url.ToString();
    }

    public string GenerateLinkHtml(MediaGenerateMarkupArgs args)
    {
      return string.Format("<a class='mf_mediaLink iframe' href='{0}' frwidth='{2}' frheight='{3}' >{1}</a>", this.GenerateFrameUrl(args), args.LinkTitle, args.Properties.Width, args.Properties.Height);
    }

    public abstract Item GetDefaultPlayer(MediaGenerateMarkupArgs args);

    public abstract string GetMediaId(Item item);
  }
}