namespace Sitecore.MediaFramework.UI.Rendering
{
  using System;

  using Sitecore.Data;
  using Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup;
  using Sitecore.MediaFramework.Players;
  using Sitecore.Web;
  using Sitecore.Web.UI.HtmlControls;
  using Sitecore.Web.UI.Sheer;

  public class EmbedLink : EmbedMedia
  {
    protected Edit LinkInput;           
    /// <summary>
    /// On Load
    /// </summary>
    /// <param name="e"></param>
    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      if (!Context.ClientPage.IsEvent && string.IsNullOrEmpty(LinkInput.Value))
      {
        this.LinkInput.Value = WebUtil.GetQueryString("link", string.Empty);
      }
    }

    /// <summary>
    /// Insert Media
    /// </summary>
    protected override void InsertMedia()
    {
      // Check validation.
      if (!this.IsValid())
      {
        return;
      }

      //TODO:var item = this.GetItem();
      //IPlayerMarkupGenerator generator = MediaFrameworkContext.GetPlayerMarkupGenerator(item);
      
      var playerProperties = new PlayerProperties
        {
          ItemId = this.SourceItemID,
          //TODO:Template = item.TemplateID,
          //MediaId = generator.GetMediaId(item),
          PlayerId = new ID(this.PlayersList.Value),
          Width = MainUtil.GetInt(this.WidthInput.Value, MediaFrameworkContext.DefaultPlayerSize.Width),
          Height = MainUtil.GetInt(this.HeightInput.Value, MediaFrameworkContext.DefaultPlayerSize.Height)
        };

      var args = new MediaGenerateMarkupArgs
      {
        MarkupType = MarkupType.Link,
        Properties = playerProperties,
        LinkTitle = this.LinkInput.Value
      };

      MediaGenerateMarkupPipeline.Run(args);
      switch (this.Mode)
      {
        case "webedit":
          SheerResponse.SetDialogValue(args.Result.Html);
          this.EndWizard();
          break;

        default:
          SheerResponse.Eval("scClose(" + StringUtil.EscapeJavascriptString(args.Result.Html) + ")");
          break;
      }
    }

    /// <summary>
    /// Checks if a form filled valid.
    /// </summary>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    protected override bool IsValid()
    {
      if (base.IsValid())
      {
        if (string.IsNullOrEmpty(this.LinkInput.Value) || string.IsNullOrWhiteSpace(this.LinkInput.Value))
        {
          SheerResponse.Alert(Translations.LinkTitleIsEmpty);
          return false;
        }
        return true;
      }
      return false;
    }
  }
}