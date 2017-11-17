namespace Sitecore.MediaFramework.Migration.Migrators
{
  using System.Collections.Generic;
  using System.Linq;

  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  public abstract class RichTextFieldMigrator : HtmlFieldMigrator
  {
    protected override IEnumerable<Field> GetFields(Item item)
    {
      Assert.ArgumentNotNull(item, "item");

      return base.GetFields(item).Where(field => field.TypeKey == "rich text");
    }
  }
}