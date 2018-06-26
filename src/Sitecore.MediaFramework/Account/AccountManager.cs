namespace Sitecore.MediaFramework.Account
{
  using System.Collections.Generic;

  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Integration.Common.Providers;

  public static class AccountManager
  {
    #region Initialization

    static AccountManager()
    {
      var helper = new ProviderHelper<AccountProvider, ProviderCollection<AccountProvider>>("mediaFramework/accountManager");

      Providers = helper.Providers;
      Provider = helper.Provider;
    }

    public static AccountProvider Provider { get; set; }

    public static ProviderCollection<AccountProvider> Providers { get; private set; }

    #endregion

    public static IEnumerable<Item> GetAllAccounts(Database database)
    {
      return Provider.GetAllAccounts(database);
    }

    public static IEnumerable<Item> GetAccountsByTemplate(Database database,ID templateId)
    {
      return Provider.GetAccountsByTemplate(database, templateId);
    }

    public static IEnumerable<Item> GetAccountsByIds(Database database, IEnumerable<ID> ids)
    {
      return Provider.GetAccountsByIds(database, ids);
    }

    public static ID GetAccountIdForDescendant(Item children)
    {
      return Provider.GetAccountIdForDescendant(children);
    }

    public static Item GetAccountItemForDescendant(Item children)
    {
      return Provider.GetAccountItemForDescendant(children);
    }

    public static Item GetAccountsRoot(Database database)
    {
      return Provider.GetAccountsRoot(database);
    }

    public static bool IsValidAccount(Item accountItem)
    {
      return Provider.IsValidAccount(accountItem);
    }

    public static List<Item> GetPlayers(Item accountItem)
    {
      return Provider.GetPlayers(accountItem);
    }

    public static Item GetSettings(Item accountItem)
    {
      return Provider.GetSettings(accountItem);
    }

    public static Field GetSettingsField(Item accountItem, ID fieldId)
    {
      return Provider.GetSettingsField(accountItem, fieldId);
    }
  }
}