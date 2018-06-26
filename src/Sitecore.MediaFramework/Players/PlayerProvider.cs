namespace Sitecore.MediaFramework.Players
{
  using System.Configuration.Provider;
  using System.Linq;
  using System.Web.UI;

  using Sitecore.Data;
  using Sitecore.Diagnostics;
  using Sitecore.Globalization;
  using Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup;

  public class PlayerProvider : ProviderBase
  {
    public virtual void RegisterDefaultResources(Page page)
    {
      Assert.ArgumentNotNull(page, "page");

      //ScriptManager.RegisterClientScriptBlock(page, typeof(Page), "MF.css", "<link rel='stylesheet' type='text/css' href='/sitecore modules/Web/MediaFramework/CSS/MF.css'>", false);
      //ScriptManager.RegisterClientScriptInclude(page, typeof(Page), "PlayerEventsListener.js", "/sitecore modules/Web/MediaFramework/js/Analytics/PlayerEventsListener.js");

      page.ClientScript.RegisterClientScriptBlock(typeof(Page), "MF.css", "<link rel='stylesheet' type='text/css' href='/sitecore modules/Web/MediaFramework/CSS/MF.css'>");
      page.ClientScript.RegisterClientScriptInclude(typeof(Page), "PlayerEventsListener.js", "/sitecore modules/Web/MediaFramework/js/Analytics/PlayerEventsListener.js");
    }

    public virtual void RegisterResources(Page page, PlayerMarkupResult result)
    {
      Assert.ArgumentNotNull(page, "page");
      Assert.ArgumentNotNull(result, "result");

      foreach (string url in result.CssUrls.Distinct())
      {
        //ScriptManager.RegisterClientScriptBlock(page, typeof(Page), url, "<link rel='stylesheet' type='text/css' href='" + url + "'>", false);
        page.ClientScript.RegisterClientScriptBlock(typeof(Page), url, "<link rel='stylesheet' type='text/css' href='" + url + "'>");
      }

      foreach (string url in result.ScriptUrls.Distinct())
      {
        //ScriptManager.RegisterClientScriptInclude(page, typeof(Page), url, url);
        page.ClientScript.RegisterClientScriptInclude(typeof(Page), url, url);
      }

      foreach (var pair in result.BottomScripts)
      {
        if (!page.ClientScript.IsStartupScriptRegistered(typeof(Page), pair.Key))
        {
          //ScriptManager.RegisterStartupScript(page, typeof(Page), pair.Key, pair.Value, true);
          page.ClientScript.RegisterStartupScript(typeof(Page), pair.Key, pair.Value, true);
        }
      }
    }

    public virtual string GetEmptyValue()
    {
      return "<div class='mf-default-view'><p>" + Translate.Text(Translations.MediaItemAreNotSelected) + "</p></div>";
    }

    public virtual string GetPreviewImage(MediaGenerateMarkupArgs args, ID imageFieldId)
    {
      string imageSrc = args.MediaItem[imageFieldId];

      if (imageSrc.Length == 0)
      {
        imageSrc = string.Format(Constants.DefaultPreview, args.Properties.Height, args.Properties.Width);
      }

      return string.Format("<img src='{0}' width='{2}' height='{3}' style=\"display:block;cursor: pointer;\" alt='{1}'/>",
        imageSrc, args.MediaItem.Name, args.Properties.Width, args.Properties.Height);
    }
  }
}