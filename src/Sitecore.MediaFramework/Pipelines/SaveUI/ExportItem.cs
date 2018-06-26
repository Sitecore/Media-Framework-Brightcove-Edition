namespace Sitecore.MediaFramework.Pipelines.SaveUI
{
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Common;
  using Sitecore.Pipelines.Save;

  public class ExportItems
  {
    public void Process(SaveArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.Items, "args.Items");

      var database = Context.ContentDatabase;
      if (!MediaFrameworkContext.IsExportAllowed(database.Name))
      {
        return;
      }

      foreach (SaveArgs.SaveItem saveItem in args.Items)
      {
        Item item = database.Items[saveItem.ID, saveItem.Language, saveItem.Version];

        item.ExportUpdate(saveItem);
      }
    }
  }
}