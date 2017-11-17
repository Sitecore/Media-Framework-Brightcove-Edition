namespace Sitecore.MediaFramework.Migration.Migrators
{
  using Sitecore.Data.Items;

  public interface IMigrator
  {
    void MigrateItem(Item item);
  }
}