namespace Sitecore.MediaFramework.Commands
{
  using System;

  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Pipelines.MediaSyncImport;

  [Serializable]
  public class ImportContent : ManualCommandBase
  {  
    protected override string JobName
    {
      get
      {
        return Translations.ImportMediaFrameworkContent;
      }
    }

    protected override Action<Item, string> Action
    {
      get
      {
        return this.Import;
      }
    }
       
    protected virtual void Import(Item accountItem, string scopeName)
    {
      MediaSyncImportPipeline.Run(new MediaSyncImportArgs { ImportName = scopeName, AccountItem = accountItem });
    }
  }
}