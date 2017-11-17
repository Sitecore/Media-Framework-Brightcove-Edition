namespace Sitecore.MediaFramework.Pipelines.MediaSyncImport
{
  using System;

  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Diagnostics;
  using Sitecore.MediaFramework.Synchronize;

  public class CallItemImport : MediaSyncImportProcessorBase
  {
    /// <summary>
    /// Call Item Export
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public override void Process(MediaSyncImportArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.AccountItem, "args.AccountItem");

      try
      {
        if (args.ResultData == null)
        {
          return;
        }

        foreach (var entity in args.ResultData)
        {
          if (entity != null)
          {
            this.SyncEntity(entity, args.AccountItem);
          }
          else
          {
            LogHelper.Debug("Entity is null.", this);
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error("Import data failed.", this, ex);
        args.AbortPipeline();
      }
    }

    protected virtual Item SyncEntity(object entity, Item accountItem)
    {
      try
      {
        IItemSynchronizer synchronizer = MediaFrameworkContext.GetItemSynchronizer(entity);
        if (synchronizer != null)
        {
          return synchronizer.SyncItem(entity, accountItem);
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error("Sync failed. Entity:"+ entity, this, ex);
        return null;
      }

      return null;
    }
  }
}