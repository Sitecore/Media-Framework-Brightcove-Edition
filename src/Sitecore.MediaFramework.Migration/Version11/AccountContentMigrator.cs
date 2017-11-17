namespace Sitecore.MediaFramework.Migration.Version11
{
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Migration.Common;
  using Sitecore.MediaFramework.Migration.Migrators;

  public class AccountContentMigrator : IMigrator
  {
    protected readonly IMigrationHelper Helper;

    public AccountContentMigrator(IMigrationHelper helper)
    {
      this.Helper = helper;
    }

    public void MigrateItem(Item item)
    {
      var synchronizer = MediaFrameworkContext.GetItemSynchronizer(item);
      
      if (synchronizer != null)
      {
        this.Helper.RecreateItem(item, true);
      }
    }
  }
}