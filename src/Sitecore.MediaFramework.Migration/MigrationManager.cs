namespace Sitecore.MediaFramework.Migration
{
  using Sitecore.Configuration;
  using Sitecore.Integration.Common.Providers;

  public static class MigrationManager
  {
    #region Initialization

    static MigrationManager()
    {
      var helper = new ProviderHelper<MigrationProvider, ProviderCollection<MigrationProvider>>("mediaFramework/migrationManager");

      Providers = helper.Providers;
    }

    public static ProviderCollection<MigrationProvider> Providers { get; private set; }

    #endregion
  }
}