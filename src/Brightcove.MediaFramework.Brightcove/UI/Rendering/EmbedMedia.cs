using Sitecore.IO;
using Sitecore.MediaFramework.Account;
using Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup;
using Sitecore.MediaFramework.UI.Rendering;
using Sitecore.MediaFramework.Utils;
using Sitecore.Web.UI.Sheer;

namespace Brightcove.MediaFramework.Brightcove.UI.Rendering
{
  using System.Globalization;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Players;
  using Sitecore.Web;
  using Sitecore.Web.UI.HtmlControls;
  using Sitecore.MediaFramework;

  public class EmbedMediaBrightcove : EmbedMedia
  {
    protected Literal SourceLiteral;

    protected Literal VideoIdLiteral;

    protected Checkbox AutoplayCheckbox;

    protected Checkbox MutedCheckbox;

    protected Radiogroup EmbedStyleRadiogroup;

    protected Radiogroup SizingRadiogroup;

    protected Combobox AspectRatioList;

    protected Combobox ShortCode;

    protected override void InitProperties()
    {
        var video = this.GetItem();
        this.WidthInput.Value = WebUtil.GetQueryString(Constants.PlayerParameters.Width, MediaFrameworkContext.DefaultPlayerSize.Width.ToString(CultureInfo.InvariantCulture));
        this.HeightInput.Value = WebUtil.GetQueryString(Constants.PlayerParameters.Height, MediaFrameworkContext.DefaultPlayerSize.Height.ToString(CultureInfo.InvariantCulture));
        this.SourceLiteral.Text = video.DisplayName;

        string player = WebUtil.GetQueryString(Constants.PlayerParameters.PlayerId, string.Empty);
         
        this.PlayerId = ShortID.IsShortID(player) ? new ShortID(player) : ID.Null.ToShortID();

        var mediaItemId = WebUtil.GetQueryString(Constants.PlayerParameters.ItemId);

        if (ID.IsID(mediaItemId))
        {
            Item item;
            Database db = Sitecore.Context.ContentDatabase ?? Sitecore.Context.Database;

            if (db != null && (item = db.GetItem(new ID(mediaItemId))) != null)
            {
                this.Filename.Value = item.Paths.MediaPath;
                this.DataContext.SetFolder(item.Uri);

                this.SourceItemID = item.ID;
                this.InitPlayersList(item);

                string activePage = WebUtil.GetQueryString(Constants.PlayerParameters.ActivePage);
                if (!string.IsNullOrEmpty(activePage))
                {
                this.Active = activePage;
                }
            }
         }
      }
      
      //TODO: Add additional params to IsValid?
      protected override bool IsValid()
      {
          string message = null;

          if (string.IsNullOrEmpty(this.PlayersList.Value))
          {
              message = Translations.PlayerIsNotSelected;
          }

          int width;
          int height;
          if (!int.TryParse(this.WidthInput.Value, out width) || width <= 0)
          {
              message = Translations.IncorrectWidthValue;
          }

          if (!int.TryParse(this.HeightInput.Value, out height) || height <= 0)
          {
              message = Translations.IncorrectHeightValue;
          }

          if (!string.IsNullOrEmpty(message))
          {
              SheerResponse.Alert(message);
              return false;
          }

          return true;
      }
      
      //TODO: create new PlayerProperties to support additional properties?
      protected override PlayerProperties GetPlayerProperties()
      {
            //TODO:var item = this.GetItem();
            //IPlayerMarkupGenerator generator = MediaFrameworkContext.GetPlayerMarkupGenerator(item);

            return new PlayerProperties
            {
                ItemId = this.SourceItemID,
                //TODO:Template = item.TemplateID,
                //MediaId = generator.GetMediaId(item),
                PlayerId = new ID(this.PlayersList.Value),
                Width = Sitecore.MainUtil.GetInt(this.WidthInput.Value, MediaFrameworkContext.DefaultPlayerSize.Width),
                Height = Sitecore.MainUtil.GetInt(this.HeightInput.Value, MediaFrameworkContext.DefaultPlayerSize.Height)
            };
      }
    }
}