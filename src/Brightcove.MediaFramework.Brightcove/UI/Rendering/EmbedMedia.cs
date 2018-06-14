using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Brightcove.MediaFramework.Brightcove.Configuration;
using Brightcove.MediaFramework.Brightcove.Entities;
using Sitecore.IO;
using Sitecore.MediaFramework.Account;
using Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup;
using Sitecore.MediaFramework.UI.Rendering;
using Sitecore.MediaFramework.Utils;
using Sitecore.Mvc.Extensions;
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
    private const string SearchItem = "SearchItem";

    protected Literal SourceLiteral;

    protected Literal VideoIdLiteral;

    protected Radiobutton EmbedStyleRadiobutton;

    protected Radiobutton SizingRadiobutton;

    protected Combobox AspectRatioList;

    protected Combobox ShortcodeList;

    protected override void InitProperties()
    {
      this.WidthInput.Value = WebUtil.GetQueryString(Constants.PlayerParameters.Width, MediaFrameworkContext.DefaultPlayerSize.Width.ToString(CultureInfo.InvariantCulture));
      this.HeightInput.Value = WebUtil.GetQueryString(Constants.PlayerParameters.Height, MediaFrameworkContext.DefaultPlayerSize.Height.ToString(CultureInfo.InvariantCulture));
      //this.EmbedStyleRadioList.SelectedIndex = Settings.DefaultVideoEmbedStyle;
      //this.SizingRadioList.SelectedIndex = Settings.DefaultVideoSizing;
      this.InitAspectRatiosList(Settings.AspectRatioList);
      this.InitShortcodeList();

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

      protected override void OnNext(object sender, EventArgs formEventArgs)
      {
          if (this.Active == SearchItem)
          {
              this.SourceItemID = this.InitMediaItem();
          }

          var item = this.GetItem();
          if (item != null && !string.IsNullOrEmpty(item.DisplayName))
          {
              this.SourceLiteral.Text = item.Name;
              this.VideoIdLiteral.Text = item["ID"];
          }

          base.OnNext(sender, formEventArgs);
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

    protected virtual void InitAspectRatiosList(IEnumerable<AspectRatio> aspectRatiosList)
    {
      if (aspectRatiosList == null || !aspectRatiosList.Any()) return;

      this.AspectRatioList.Controls.Clear();
      foreach (var aspectRatio in aspectRatiosList.ToList())
      {
        this.AspectRatioList.Controls.Add(new ListItem
        {
          ID = Control.GetUniqueID("ListItem"),
          Selected = aspectRatio.Height == Settings.AspectRatioList.FirstOrDefault().Height,
          Header = aspectRatio.DisplayName,
          Value = aspectRatio.DisplayName
        });
      }
    }

    protected virtual void InitShortcodeList()
    {
      this.ShortcodeList.Controls.Clear();

      this.ShortcodeList.Controls.Add(new ListItem
      {
        ID = Control.GetUniqueID("ListItem"),
        Selected = true,
        Header = "Manual",
        Value = "manual"
      });
      this.ShortcodeList.Controls.Add(new ListItem
      {
        ID = Control.GetUniqueID("ListItem"),
        Selected = true,
        Header = "Auto generate",
        Value = "auto"
      });
    }
  }
}