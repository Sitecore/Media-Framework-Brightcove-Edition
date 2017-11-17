namespace Sitecore.MediaFramework.Commands
{
  using System;

  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Pipelines.MediaCleanup;

  [Serializable]
  public class CleanupContent : ManualCommandBase
  {
    protected override string JobName
    {
      get
      {
        return Translations.CleanupMediaFrameworkContent;
      }
    }

    protected override Action<Item, string> Action
    {
      get
      {
        return this.Cleanup;
      }
    }

    protected virtual void Cleanup(Item accountItem, string scopeName)
    {
      MediaCleanupPipeline.Run(new MediaCleanupArgs { CleanupExecuterName = scopeName, AccountItem = accountItem });
    }
  }
}