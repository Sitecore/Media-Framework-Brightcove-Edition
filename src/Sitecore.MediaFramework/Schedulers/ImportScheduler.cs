namespace Sitecore.MediaFramework.Schedulers
{
  using System;

  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Pipelines.MediaSyncImport;

  public class ImportScheduler : ScopeSchedulerBase
  {
    public ImportScheduler(string database) : base(database)
    {
    }

    public ImportScheduler(string database, string userName)
      : base(database, userName)
    {
    }

    public override Action<Item, string> ScopeAction
    {
      get
      {
        return this.Import;
      }
    }

    protected virtual void Import(Item accountItem, string scopeName)
    {
      MediaSyncImportPipeline.Run(
        new MediaSyncImportArgs
          {
            ImportName = scopeName,
            AccountItem = accountItem,
          });
    }
  }
}