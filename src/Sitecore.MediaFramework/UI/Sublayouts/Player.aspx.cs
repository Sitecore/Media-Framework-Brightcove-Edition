namespace Sitecore.MediaFramework.UI.Sublayouts
{
  using System;
  using System.Web.UI;

  using Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup;
  using Sitecore.MediaFramework.Players;

  public partial class Player : Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (Page.IsPostBack)
      {
        return;
      }

      PlayerProperties properties = new PlayerProperties(this.Request.QueryString);

      var args = new MediaGenerateMarkupArgs
        {
          MarkupType = MarkupType.Html,
          Properties = properties
        };

      MediaGenerateMarkupPipeline.Run(args);

      PlayerManager.RegisterDefaultResources(this.Page);

      if (!args.Aborted)
      {
        this.PlayerContainer.InnerHtml = args.Result.Html;
        this.PlayerContainer.Attributes["data-mf-params"] = properties.ToString();

        PlayerManager.RegisterResources(this.Page, args.Result);
      }
      else
      {
        this.PlayerContainer.InnerHtml = PlayerManager.GetEmptyValue();
      }
    }
  }
}