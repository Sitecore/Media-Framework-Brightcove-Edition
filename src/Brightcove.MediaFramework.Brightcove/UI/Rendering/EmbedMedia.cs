using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Brightcove.MediaFramework.Brightcove.Configuration;
using Brightcove.MediaFramework.Brightcove.Entities;
using Sitecore.Diagnostics;
using Sitecore.IO;
using Sitecore.MediaFramework.Account;
using Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup;
using Sitecore.MediaFramework.UI.Rendering;
using Sitecore.MediaFramework.Utils;
using Sitecore.Mvc.Extensions;
using Sitecore.Text;
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

    protected Edit SourceInput;

    protected Edit VideoIdInput;

    protected Checkbox AutoplayCheckbox;
    protected Checkbox MutedCheckbox;
    
    protected Radiobutton JavascriptRadiobutton;
    protected Radiobutton IframeRadiobutton;
    protected Edit EmbedInput;
    
    protected Radiobutton ResponsiveRadiobutton;
    protected Radiobutton FixedRadiobutton;
    protected Edit SizingInput;

    protected Combobox AspectRatioList;

    protected override void InitProperties()
    {
      this.WidthInput.Value = WebUtil.GetQueryString(Constants.PlayerParameters.Width, MediaFrameworkContext.DefaultPlayerSize.Width.ToString(CultureInfo.InvariantCulture));
      this.HeightInput.Value = WebUtil.GetQueryString(Constants.PlayerParameters.Height, MediaFrameworkContext.DefaultPlayerSize.Height.ToString(CultureInfo.InvariantCulture));
      this.InitAspectRatiosList();
      this.JavascriptRadiobutton.Checked = true;
      this.EmbedInput.Value = Brightcove.Constants.EmbedJavascript;
      this.ResponsiveRadiobutton.Checked = true;
      this.SizingInput.Value = Brightcove.Constants.SizingResponsive;
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

    private void button1_Click(object sender, EventArgs e)
    {
      this.JavascriptRadiobutton.Checked = !this.JavascriptRadiobutton.Checked;
      this.IframeRadiobutton.Checked = !this.IframeRadiobutton.Checked;
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
        this.SourceInput.Value = item.DisplayName;
        this.SourceInput.Disabled = true;
        this.VideoIdInput.Value = item["ID"];
        this.VideoIdInput.Disabled = true;
        SheerResponse.Eval("scNext()");
      }

      base.OnNext(sender, formEventArgs);
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
    
    protected override PlayerProperties GetPlayerProperties()
    {
      //TODO:var item = this.GetItem();
      //IPlayerMarkupGenerator generator = MediaFrameworkContext.GetPlayerMarkupGenerator(item);

      var properties = new Dictionary<string, string>();
      if (this.AutoplayCheckbox != null && this.AutoplayCheckbox.Checked)
        properties.Add(BrightcovePlayerParameters.Autoplay, this.AutoplayCheckbox.Value);
      if (this.MutedCheckbox != null && this.MutedCheckbox.Checked)
        properties.Add(BrightcovePlayerParameters.Muted, this.MutedCheckbox.Value);
      //var embed = this.IframeRadiobutton.Checked
      //  ? Brightcove.Constants.EmbedJavascript
      //  : Brightcove.Constants.EmbedIframe;
      properties.Add(BrightcovePlayerParameters.EmbedStyle, this.EmbedInput.Value);
      //if (this.FixedRadiobutton != null && !this.FixedRadiobutton.Checked)
      properties.Add(BrightcovePlayerParameters.Sizing, this.SizingInput.Value);
      properties.Add(BrightcovePlayerParameters.AspectRatio, this.AspectRatioList.Selected.FirstOrDefault().Value);

      var playerProps = new PlayerProperties(properties);
      playerProps.ItemId = this.SourceItemID;
      //TODO:playerProps.Template = item.TemplateID;
      //playerProps.MediaId = generator.GetMediaId(item);
      playerProps.PlayerId = !string.IsNullOrWhiteSpace(this.PlayersList?.Value) ? new ID(this.PlayersList.Value) : new ID();
      playerProps.Width = Sitecore.MainUtil.GetInt(this.WidthInput.Value, MediaFrameworkContext.DefaultPlayerSize.Width);
      playerProps.Height = Sitecore.MainUtil.GetInt(this.HeightInput.Value, MediaFrameworkContext.DefaultPlayerSize.Height);
      return playerProps;
    }

    protected virtual void InitAspectRatiosList()
    {
      this.AspectRatioList.Controls.Clear();
      this.HeightInput.Disabled = true;
      this.AspectRatioList.Controls.Add(new ListItem
      {
        ID = Control.GetUniqueID("ListItem"),
        Selected = true,
        Header = BrightcovePlayerParameters.Ratio16X9,
        Value = BrightcovePlayerParameters.Ratio16X9
      });
      this.AspectRatioList.Controls.Add(new ListItem
      {
        ID = Control.GetUniqueID("ListItem"),
        Selected = false,
        Header = BrightcovePlayerParameters.Ratio4X3,
        Value = BrightcovePlayerParameters.Ratio4X3
      });
      this.AspectRatioList.Controls.Add(new ListItem
      {
        ID = Control.GetUniqueID("ListItem"),
        Selected = false,
        Header = BrightcovePlayerParameters.RatioCustom,
        Value = BrightcovePlayerParameters.RatioCustom
      });
    }
  }
}