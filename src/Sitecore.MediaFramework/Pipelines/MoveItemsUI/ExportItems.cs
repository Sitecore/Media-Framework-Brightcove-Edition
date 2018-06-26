
namespace Sitecore.MediaFramework.Pipelines.MoveItemsUI
{
  using System.Collections.Generic;
  using System.Linq;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Common;
  using Sitecore.Web.UI.Sheer;

  public class ExportItems
  {
    public void Process(ClientPipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.Parameters, "args.Parameters");

      if (args.Parameters["copy"] == "1" || !MediaFrameworkContext.IsExportAllowed(args.Parameters["database"]))
      {
        return;
      }

      foreach (Item item in this.GetItems(args))
      {
        item.ExportMove();
      }
    }

    protected virtual IEnumerable<Item> GetItems(ClientPipelineArgs args)
    {
      Database database = Factory.GetDatabase(args.Parameters["database"]);
      return ID.ParseArray(args.Parameters["items"]).Select(database.GetItem).Where(item => item != null);
    }
  }
}