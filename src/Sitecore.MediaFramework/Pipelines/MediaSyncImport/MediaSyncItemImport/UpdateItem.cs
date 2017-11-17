namespace Sitecore.MediaFramework.Pipelines.MediaSyncImport.MediaSyncItemImport
{
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Diagnostics;

  public class UpdateItem : MediaSyncItemImportProcessorBase
  {
    public override void Process(MediaSyncItemImportArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.Entity, "args.Entity");
      Assert.ArgumentNotNull(args.AccountItem, "args.AccountItem");
      Assert.ArgumentNotNull(args.Synchronizer, "args.Synchronizer");

      if (!this.ValidateActivity(args, SyncAllowActivity.UpdateItem))
      {
        return;
      }

      Item item = this.GetItemForUpdate(args);

      if (this.CheckEdit(item))
      {
        args.Item = args.Synchronizer.UpdateItem(args.Entity, args.AccountItem, item);
      }
    }

    protected virtual Item GetItemForUpdate(MediaSyncItemImportArgs args)
    {
      if (args.Item != null)
      {
        return args.Item;
      }

      if (args.SearchResultItem != null)
      {
        if (args.Synchronizer.NeedUpdate(args.Entity, args.AccountItem, args.SearchResultItem))
        {
          LogHelper.Debug("NeedUpdate returns 'true'. SearchResult:" + args.SearchResultItem.Uri, this);

          return args.SearchResultItem.GetItem();
        }
        LogHelper.Debug("NeedUpdate returns 'false'. SearchResult:" + args.SearchResultItem.Uri, this);
      }
      else
      {
        LogHelper.Debug("SearchResultItem is null. Entity:" + args.Entity, this);
      }

      return null;
    }
  }
}