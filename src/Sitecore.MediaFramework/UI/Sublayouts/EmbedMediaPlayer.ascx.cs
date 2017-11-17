namespace Sitecore.MediaFramework.UI.Sublayouts
{                     
  using System;
  using System.Web.UI;

  using Sitecore.Data;
  using Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup;
  using Sitecore.MediaFramework.Players;
  using Sitecore.Web.UI.WebControls;

  public partial class EmbedMediaPlayer : UserControl
  {                                                                  
    protected void Page_Load(object sender, EventArgs e)
    {
      //if (Page.IsPostBack)
      //{
      //  return;
      //}

      PlayerManager.RegisterDefaultResources(this.Page);

      var sub = this.Parent as Sublayout;
      if (sub != null && Sitecore.Data.ID.IsID(sub.DataSource))
      {
        var properties = new PlayerProperties(StringUtil.GetNameValues(sub.Parameters, '=', '&'))
          {
            ItemId = new ID(sub.DataSource)
          };

        var args = new MediaGenerateMarkupArgs
          {
            MarkupType = MarkupType.Html,
            Properties = properties,
          };

        MediaGenerateMarkupPipeline.Run(args);

        if (!args.Aborted)
        {
          this.PlayerContainer.InnerHtml = args.Result.Html;

          PlayerManager.RegisterResources(this.Page, args.Result);

          this.PlayerContainer.Attributes["data-mf-params"] = properties.ToString();
        }
        else
        {
          this.PlayerContainer.Attributes.Remove("data-mf-params");
          this.PlayerContainer.InnerHtml = PlayerManager.GetEmptyValue();
        }
      }
      else
      {
        this.PlayerContainer.InnerHtml = PlayerManager.GetEmptyValue();
      }
    }
  }
}