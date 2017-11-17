namespace Sitecore.MediaFramework.Pipelines.MediaSyncImport.MediaSyncItemImport
{
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Diagnostics;

  public class DatabaseFallback : MediaSyncItemImportProcessorBase
  {
    public override void Process(MediaSyncItemImportArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.Entity, "args.Entity");
      Assert.ArgumentNotNull(args.AccountItem, "args.AccountItem");
      Assert.ArgumentNotNull(args.Synchronizer, "args.Synchronizer");

      if (!this.ValidateActivity(args, SyncAllowActivity.DatabaseFallback))
      {
        return;
      }

      if (args.SearchResultItem == null && args.Item == null)
      {
        args.SearchResultItem = args.Synchronizer.Fallback(args.Entity, args.AccountItem);

        if (args.SearchResultItem != null)
        {
          LogHelper.Debug("Search result item has been found by fallback. SearchResult:" + args.SearchResultItem.Uri, this);
        }
        else
        {
          LogHelper.Debug("Search result item does not found by fallback. Entity:" + args.Entity, this);
        }
      }
    }
  }
}