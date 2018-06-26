namespace Sitecore.MediaFramework.Migration.Migrators
{
  using Sitecore.Data.Fields;

  public interface IFieldMigrator : IMigrator
  {
    void MigrateField(Field field);
  }
}