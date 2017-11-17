namespace Sitecore.MediaFramework.Pipelines.MediaSyncImport.MediaSyncItemImport
{
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Entities;
  using Sitecore.MediaFramework.Pipelines.Common;
  using Sitecore.MediaFramework.Synchronize;
  using Sitecore.Pipelines;

  public class MediaSyncItemImportArgs : PipelineArgs, IItemArgs
  {
    /// <summary>
    /// Gets or sets the account item.
    /// </summary>
    public Item AccountItem { get; set; }

    /// <summary>
    /// Gets or sets the data.
    /// </summary>
    public object Entity { get; set; }

    public Item Item { get; set; }

    public IItemSynchronizer Synchronizer { get; set; }

    public MediaServiceSearchResult SearchResultItem { get; set; }

    public SyncAllowActivity SyncAllowActivity { get; set; }
  }
}