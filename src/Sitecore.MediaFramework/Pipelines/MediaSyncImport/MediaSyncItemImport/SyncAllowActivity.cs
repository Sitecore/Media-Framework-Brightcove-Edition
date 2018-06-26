namespace Sitecore.MediaFramework.Pipelines.MediaSyncImport.MediaSyncItemImport
{
  using System;

  [Flags]
  public enum SyncAllowActivity
  {
    IndexSearch = 1,
    DatabaseFallback = 2,
    CreateItem = 4,
    UpdateItem = 8,
    SyncReferences = 16,
    All = 31
  }
}