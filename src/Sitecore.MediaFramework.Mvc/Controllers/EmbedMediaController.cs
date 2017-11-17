namespace Sitecore.MediaFramework.Mvc.Controllers
{
  using System.Linq;
  using System.Web.Mvc;

  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup;
  using Sitecore.MediaFramework.Players;
  using Sitecore.Mvc.Presentation;

  public class EmbedMediaController : Controller
  {
    private string renderVideoViewPath;

    protected virtual string RenderVideoViewPath
    {
      get
      {
        return this.renderVideoViewPath ?? (this.renderVideoViewPath = Settings.GetSetting("Sitecore.MediaFramework.Mvc.EmbedMediaViewPath"));
      }
    }

    public ActionResult RenderVideo()
    {
      Rendering rendering = RenderingContext.Current.Rendering;

      if (!ID.IsID(rendering.DataSource))
      {
        var emptyArgs = new MediaGenerateMarkupArgs();
        emptyArgs.AbortPipeline();

        return this.View(this.RenderVideoViewPath, emptyArgs);
      }

      PlayerProperties properties = new PlayerProperties(rendering.Parameters.ToDictionary(p => p.Key, p => p.Value))
      {
        ItemId = new ID(rendering.DataSource)
      };

      var args = new MediaGenerateMarkupArgs
      {
        MarkupType = MarkupType.Html,
        Properties = properties,
      };

      MediaGenerateMarkupPipeline.Run(args);

      return this.View(this.RenderVideoViewPath, args);
    }
  }
}