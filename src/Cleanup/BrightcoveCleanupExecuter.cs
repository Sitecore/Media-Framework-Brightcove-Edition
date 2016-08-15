namespace Sitecore.MediaFramework.Brightcove.Cleanup
{
  using System;

  using Sitecore.MediaFramework.Cleanup;
  using Sitecore.MediaFramework.Entities;

  [Obsolete("Use Sitecore.MediaFramework.Cleanup.CleanupExecuterBase class")]
  public abstract class BrightcoveCleanupExecuter<TEntity, TSearchResult> : CleanupExecuterBase<TEntity, TSearchResult>
    where TSearchResult : MediaServiceSearchResult, new()
  {
  }
}