namespace Sitecore.MediaFramework.UI.Rendering
{
  using System;
  using System.Globalization;

  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Globalization;
  using Sitecore.IO;
  using Sitecore.MediaFramework.Account;
  using Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup;
  using Sitecore.MediaFramework.Players;
  using Sitecore.MediaFramework.Utils;
  using Sitecore.Web;
  using Sitecore.Web.UI.HtmlControls;
  using Sitecore.Web.UI.Pages;
  using Sitecore.Web.UI.Sheer;

  public class EmbedMedia : WizardForm
  {
    private const string QueryMode = "mo";
    private const string IsPageEdit = "pe";

    private const string SourceFolder = "fo";
    private const string SearchItem = "SearchItem";

    private const string ParameterSetter = "ParameterSetter";

    private ID sourceItemID;

    protected DataContext DataContext;

    protected Edit Filename;

    protected Frame UploadFrame;

    protected Edit WidthInput;

    protected Edit HeightInput;

    protected Combobox PlayersList;

    protected ID SourceItemID
    {
      get
      {
        return this.sourceItemID ?? (this.sourceItemID = this.ServerProperties["itemID"] as ID);
      }

      set
      {
        this.ServerProperties["itemID"] = this.sourceItemID = value;
      }
    }

    // Properties
    protected Language ContentLanguage
    {
      get
      {
        Language contentLanguage;
        if (!Language.TryParse(WebUtil.GetQueryString("la"), out contentLanguage))
        {
          contentLanguage = Context.ContentLanguage;
        }
        return contentLanguage;
      }
    }

    protected string Mode
    {
      get
      {
        return Assert.ResultNotNull(StringUtil.GetString(this.ServerProperties[QueryMode], "shell"));
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");
        this.ServerProperties[QueryMode] = value;
      }
    }

    protected ShortID PlayerId
    {
      get
      {
        string str = StringUtil.GetString(this.ServerProperties[Constants.PlayerParameters.PlayerId], string.Empty);

        ShortID id;

        return ShortID.TryParse(str, out id)? id : ID.Null.ToShortID();
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.ServerProperties[Constants.PlayerParameters.PlayerId] = !ReferenceEquals(value, null) ? value.ToString() : null;
      }
    }

    protected override void OnLoad(EventArgs e)
    {
      Assert.ArgumentNotNull(e, "e");
      base.OnLoad(e);
      if (!Context.ClientPage.IsEvent)
      {
        this.Mode = WebUtil.GetQueryString(QueryMode);
        this.InitProperties();

        this.DataContext.GetFromQueryString();
        this.UploadFrame.SourceUri += string.Format("?type=embed&database={0}", this.DataContext.GetDatabase());
        string queryString = WebUtil.GetQueryString(SourceFolder);
        if (ShortID.IsShortID(queryString))
        {
          queryString = ShortID.Parse(queryString).ToID().ToString();
          this.DataContext.Folder = queryString;
        }

        var folder = this.DataContext.GetFolder();
        Assert.IsNotNull(folder, "Folder not found");
      }
    }

    protected virtual void InitProperties()
    {
      this.WidthInput.Value = WebUtil.GetQueryString(Constants.PlayerParameters.Width, MediaFrameworkContext.DefaultPlayerSize.Width.ToString(CultureInfo.InvariantCulture));
      this.HeightInput.Value = WebUtil.GetQueryString(Constants.PlayerParameters.Height, MediaFrameworkContext.DefaultPlayerSize.Height.ToString(CultureInfo.InvariantCulture));

      string player = WebUtil.GetQueryString(Constants.PlayerParameters.PlayerId, string.Empty);
     
      this.PlayerId = ShortID.IsShortID(player) ? new ShortID(player) : ID.Null.ToShortID(); 

      var mediaItemId = WebUtil.GetQueryString(Constants.PlayerParameters.ItemId);

      if (ID.IsID(mediaItemId))
      {
        Item item;
        Database db = Context.ContentDatabase ?? Context.Database;

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

    protected override void OnCancel(object sender, EventArgs args)
    {
      Assert.ArgumentNotNull(sender, "sender");
      Assert.ArgumentNotNull(args, "args");

      if (this.Active == ParameterSetter)
      {
        if (this.IsValid())
        {
          this.InsertMedia();
        }

        return;
      }

      if (this.Mode == "webedit")
      {
        base.OnCancel(sender, args);
      }
      else
      {
        SheerResponse.Eval("scCancel()");
      }
    }

    protected virtual bool IsValid()
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

      base.OnNext(sender, formEventArgs);
    }

    protected virtual PlayerProperties GetPlayerProperties()
    {
      //TODO:var item = this.GetItem();
      //IPlayerMarkupGenerator generator = MediaFrameworkContext.GetPlayerMarkupGenerator(item);

      return new PlayerProperties
      {
        ItemId = this.SourceItemID,
        //TODO:Template = item.TemplateID,
        //MediaId = generator.GetMediaId(item),
        PlayerId = new ID(this.PlayersList.Value),
        Width = MainUtil.GetInt(this.WidthInput.Value, MediaFrameworkContext.DefaultPlayerSize.Width),
        Height = MainUtil.GetInt(this.HeightInput.Value, MediaFrameworkContext.DefaultPlayerSize.Height)
      };
    }

    protected virtual void InsertMedia()
    {
      this.InsertMedia(this.GetPlayerProperties());
    }

    protected virtual Item GetItem()
    {
      string str = this.Filename.Value;
      if (str.Length == 0)
      {
        return null;
      }

      if (ID.IsID(str))
      {
        return this.DataContext.GetItem(new ID(str));
      }

      Item root = this.DataContext.GetRoot();
      if (root != null)
      {
        Item rootItem = root.Database.GetRootItem();
        if ((rootItem != null) && (root.ID != rootItem.ID))
        {
          str = FileUtil.MakePath(root.Paths.Path, str, '/');
        }
      }

      return this.DataContext.GetItem(str);
    }

    protected virtual ID InitMediaItem()
    {
      var item = this.GetItem();
      if (item == null)
      {
        SheerResponse.Alert(Translations.MediaItemCouldNotBeFound);
      }
      else if (!MediaItemUtil.IsMediaElement(item.Template))
      {
        SheerResponse.Alert(Translations.SelectedItemIsNotMediaElement);
      }
      else
      {
        this.InitPlayersList(item);
        return item.ID;
      }

      this.Back();
      return ID.Null;
    }

    protected virtual void InsertMedia(PlayerProperties playerProperties)
    {
      if (playerProperties == null)
      {
        return;
      }

      var args = new MediaGenerateMarkupArgs
      {
        MarkupType = MarkupType.Frame,
        Properties = playerProperties
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

    protected virtual void InitPlayersList(Item item)
    {
      IPlayerMarkupGenerator generator = MediaFrameworkContext.GetPlayerMarkupGenerator(item);
      if (generator == null)
      {
        return;
      }

      Item account = AccountManager.GetAccountItemForDescendant(item);
      if (account == null)
      {
        return;
      }

      var players = AccountManager.GetPlayers(account);

      this.PlayersList.Controls.Clear();
      foreach (var playerItem in players)
      {
        this.PlayersList.Controls.Add(new ListItem
          {
            ID = Control.GetUniqueID("ListItem"),
            Selected = this.PlayerId == playerItem.ID.ToShortID(),
            Header = playerItem.DisplayName,
            Value = playerItem.ID.ToShortID().ToString()
          });
      }
      Context.ClientPage.ClientResponse.Refresh(this.PlayersList);
    }
  }
}