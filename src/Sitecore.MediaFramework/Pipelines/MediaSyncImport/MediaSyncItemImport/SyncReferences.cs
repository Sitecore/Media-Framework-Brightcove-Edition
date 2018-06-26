namespace Sitecore.MediaFramework.Pipelines.MediaSyncImport.MediaSyncItemImport
{
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Diagnostics;

  public class SyncReferences : MediaSyncItemImportProcessorBase
  {
    public override void Process(MediaSyncItemImportArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.Entity, "args.Entity");
      Assert.ArgumentNotNull(args.AccountItem, "args.AccountItem");
      Assert.ArgumentNotNull(args.Synchronizer, "args.Synchronizer");

      if (!this.ValidateActivity(args, SyncAllowActivity.SyncReferences))
      {
        return;
      }

      if (args.Synchronizer.References.Count > 0)
      {
        LogHelper.Debug(string.Format("{0} has {1} reference synchronizers", args.Synchronizer.GetType().Name, args.Synchronizer.References.Count), this);
      }
      else
      {
        return;
      }

      Item item = this.GetItem(args);
      if (this.CheckEdit(item))
      {
        LogHelper.Debug(string.Format("Item({0}) references sync has been started", item.ID), this);

        foreach (var reference in args.Synchronizer.References)
        {
          Item syncItem = reference.SyncReference(args.Entity, args.AccountItem, item);

          LogHelper.Debug(string.Format("Item({0}) reference has been sync by: {1}.", item.ID, reference.GetType().Name), this);

          if (args.Item == null)
          {
            args.Item = syncItem;
          }
        }

        //args.Item = args.Synchronizer.SyncReferences(args.Entity, args.AccountItem, item) ?? args.Item;
      }
      else
      {
        LogHelper.Debug("Item is null. Entity:" + args.Entity, this);
      }
    }

    protected virtual Item GetItem(MediaSyncItemImportArgs args)
    {
      if (args.Item != null)
      {
        return args.Item;
      }

      if (args.SearchResultItem != null)
      {
        //TODO:try catch & logs
        return args.SearchResultItem.GetItem();
      }

      return null;
    }
  }
}