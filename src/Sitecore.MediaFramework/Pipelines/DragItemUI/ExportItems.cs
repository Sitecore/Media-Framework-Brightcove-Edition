namespace Sitecore.MediaFramework.Pipelines.DragItemUI
{
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

      Item item = this.GetItem(args);
      item.ExportMove();
    }

    protected virtual Item GetItem(ClientPipelineArgs args)
    {
      Database database = Factory.GetDatabase(args.Parameters["database"]);
      return database.GetItem(args.Parameters["id"]);
    }
  }
}