namespace Sitecore.MediaFramework.Account
{
  using System;
  using System.Collections.Generic;
  using System.Configuration.Provider;
  using System.Linq;

  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Common;

  public class AccountProvider : ProviderBase
  {
    public virtual IEnumerable<Item> GetAllAccounts(Database database)
    {
      return this.GetAccounts(database, "./*");
    }

    public virtual IEnumerable<Item> GetAccountsByTemplate(Database database, ID templateId)
    {
      return this.GetAccounts(database, string.Format("./*[@@templateid='{0}']", templateId));
    }

    public virtual IEnumerable<Item> GetAccountsByIds(Database database, IEnumerable<ID> ids)
    {
      Assert.ArgumentNotNull(database, "database");
      Assert.ArgumentNotNull(ids, "ids");

      return ids.Select(database.GetItem).Where(item => item != null);
    }

    public virtual ID GetAccountIdForDescendant(Item children)
    {
      Assert.ArgumentNotNull(children, "children");

      string[] idPath = children.Paths.GetPath(ItemPathType.ItemID).Split('/');

      if (idPath.Length > 5 && idPath[4].Equals(ItemIDs.AccountsRoot.ToString(), StringComparison.OrdinalIgnoreCase))
      {
        return new ID(idPath[5]);
      }

      return null;
    }

    public virtual Item GetAccountItemForDescendant(Item children)
    {
      Assert.ArgumentNotNull(children, "children");

      ID accountId = this.GetAccountIdForDescendant(children);
      Item accountItem;
      if (!ReferenceEquals(accountId, null) && (accountItem = children.Database.GetItem(accountId)) != null)
      {
        return accountItem;
      }
                                                                                                              
      return null;
    }

    public virtual Item GetAccountsRoot(Database database)
    {
      Assert.ArgumentNotNull(database, "database");

      return database.GetItem(ItemIDs.AccountsRoot);
    }

    public virtual bool IsValidAccount(Item accountItem)
    {
      return accountItem.IsValidItem();
    }

    public virtual List<Item> GetPlayers(Item accountItem)
    {
      Assert.ArgumentNotNull(accountItem, "accountItem");

      Item playersRootItem = accountItem.Children["Players"];

      return playersRootItem != null ? playersRootItem.Children.ToList() : new List<Item>(0);
    }

    public virtual Item GetSettings(Item accountItem)
    {
      Assert.ArgumentNotNull(accountItem, "accountItem");

      return accountItem.Children["Settings"];
    }

    public virtual Field GetSettingsField(Item accountItem,ID fieldId)
    {
      Item settings = this.GetSettings(accountItem);
      return settings != null ? settings.Fields[fieldId] : null;
    }

    protected virtual IEnumerable<Item> GetAccounts(Database database, string query)
    {
      Assert.ArgumentNotNull(database, "database");
      Assert.ArgumentNotNullOrEmpty(query, "query");

      Item accountsRoot = this.GetAccountsRoot(database);
      if (accountsRoot == null)
      {
        return Enumerable.Empty<Item>();
      }

      if (!query.StartsWith("./"))
      {
        query = "./" + query;
      }

      return accountsRoot.Axes.SelectItems(query) ?? Enumerable.Empty<Item>();
    }
  }
}