
namespace Sitecore.MediaFramework.Schedulers
{
  using System;

  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Pipelines.MediaCleanup;

  public class CleanupScheduler : ScopeSchedulerBase
  {
    public CleanupScheduler(string database) : base(database)
    {
    }

    public CleanupScheduler(string database, string userName)
      : base(database, userName)
    {
    }

    public override Action<Item, string> ScopeAction
    {
      get
      {
        return this.Cleanup;
      }
    }

    protected virtual void Cleanup(Item accountItem, string scopeName)
    {
      MediaCleanupPipeline.Run(new MediaCleanupArgs
        {
          AccountItem = accountItem,
          CleanupExecuterName = scopeName
        });
    }
  }
}