namespace Sitecore.MediaFramework.Pipelines.MediaSyncImport.MediaSyncItemImport
{
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Diagnostics;

  public class IndexSearch : MediaSyncItemImportProcessorBase
  {
    public override void Process(MediaSyncItemImportArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.Entity, "args.Entity");
      Assert.ArgumentNotNull(args.AccountItem, "args.AccountItem");
      Assert.ArgumentNotNull(args.Synchronizer, "args.Synchronizer");

      if (!this.ValidateActivity(args, SyncAllowActivity.IndexSearch))
      {
        return;
      }

      if (args.SearchResultItem == null && args.Item == null)
      {
        args.SearchResultItem = args.Synchronizer.GetSearchResult(args.Entity, args.AccountItem);

        if (args.SearchResultItem != null)
        {
          LogHelper.Debug("Search result item has been found by indexing. SearchResult:" + args.SearchResultItem.Uri, this);
        }
        else
        {
          LogHelper.Debug("Search result item does not found by indexing. Entity:" + args.Entity, this);
        }
      }
    }
  }
}