
namespace Sitecore.MediaFramework.Scopes
{
  using System;
  using System.Collections.Generic;

  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Integration.Common.Providers;
  using Sitecore.MediaFramework.Schedulers;

  public static class ScopeManager
  {
    static ScopeManager()
    {
      var helper = new ProviderHelper<ScopeProvider, ProviderCollection<ScopeProvider>>("mediaFramework/scopeManager");
      Providers = helper.Providers;
      Provider = helper.Provider;
    }

    public static ScopeProvider Provider { get; set; }

    public static ProviderCollection<ScopeProvider> Providers { get; private set; }

    public static ScopeExecuteConfiguration GetExecuteConfiguration(string name)
    {
      return Provider.GetExecuteConfiguration(name);
    }

    public static void Execute(Database database, string name, Action<Item, string> scopeAction, IEnumerable<ID> exactAccounts = null)
    {
      Provider.Execute(database, name, scopeAction, exactAccounts);
    }

    public static void Execute(Database database, ScopeExecuteConfiguration executeConfiguration, Action<Item, string> scopeAction, IEnumerable<ID> exactAccounts = null)
    {
      Provider.Execute(database, executeConfiguration, scopeAction, exactAccounts);
    }
  }
}