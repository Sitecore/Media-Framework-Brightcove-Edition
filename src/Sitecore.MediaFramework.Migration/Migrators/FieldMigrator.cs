namespace Sitecore.MediaFramework.Migration.Migrators
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Diagnostics;

  public abstract class FieldMigrator : IFieldMigrator
  {
    public virtual void MigrateItem(Item item)
    {
      Assert.ArgumentNotNull(item, "item");

      LogHelper.Info(string.Format("{0}({1}) item migration",item.Name,item.ID),this);

      var fields = this.GetFields(item) ?? Enumerable.Empty<Field>();

      foreach (Field field in fields)
      {
        LogHelper.Info(string.Format("{0}({1}) '{2}' field migration", item.Name, item.ID, field.Name), this);

        try
        {
          this.MigrateField(field);
        }
        catch (Exception ex)
        {
          LogHelper.Error(string.Format("{0}({1}) '{2}' field migration error", item.Name, item.ID, field.Name), this, ex);
        }
      }
    }

    public abstract void MigrateField(Field field);

    protected virtual IEnumerable<Field> GetFields(Item item)
    {
      Assert.ArgumentNotNull(item, "item");

      return item.Fields;
    }
  }
}