namespace Sitecore.MediaFramework.Players
{
  using System.Web.UI;

  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Integration.Common.Providers;
  using Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup;

  public static class PlayerManager
  {
    #region Initialization

    static PlayerManager()
    {
      var helper = new ProviderHelper<PlayerProvider, ProviderCollection<PlayerProvider>>("mediaFramework/playerManager");
      Providers = helper.Providers;
      Provider = helper.Provider;
    }

    public static PlayerProvider Provider { get; set; }

    public static ProviderCollection<PlayerProvider> Providers { get; private set; }

    #endregion

    public static void RegisterDefaultResources(Page page)
    {
      Provider.RegisterDefaultResources(page);
    }

    public static void RegisterResources(Page page, PlayerMarkupResult result)
    {
      Provider.RegisterResources(page, result);
    }

    public static string GetEmptyValue()
    {
      return Provider.GetEmptyValue();
    }

    public static string GetPreviewImage(MediaGenerateMarkupArgs args, ID imageFieldId)
    {
      return Provider.GetPreviewImage(args, imageFieldId);
    }
  }
}