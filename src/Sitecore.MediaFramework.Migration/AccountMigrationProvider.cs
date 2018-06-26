namespace Sitecore.MediaFramework.Migration
{
  using System.Collections.Generic;

  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Integration.Common.Extensions;
  using Sitecore.MediaFramework.Account;

  public class AccountMigrationProvider : MigrationProvider
  {
    public override IEnumerable<Item> GetItems(Item rootItem, bool isDeep)
    {
      return this.GetFoldersContent(this.GetFolders(rootItem));
    }

    protected virtual IEnumerable<Item> GetFolders(Item rootItem)
    {
      Item accountItem = AccountManager.GetAccountItemForDescendant(rootItem);
      if (accountItem == null)
      {
        yield break;
      }

      yield return accountItem.Children["Media Content"];
      yield return accountItem.Children["Players"];
    }

    protected virtual IEnumerable<Item> GetFoldersContent(IEnumerable<Item> folders)
    {
      Assert.ArgumentNotNull(folders, "folders");

      foreach (Item folder in folders)
      {
        if (folder == null)
        {
          continue;
        }

        foreach (Item item in folder.GetDescendants())
        {
          var synchronizer = MediaFrameworkContext.GetItemSynchronizer(item);

          if (synchronizer != null)
          {
            yield return item;
          }
        }
      }

    }
  }
}