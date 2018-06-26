namespace Sitecore.MediaFramework.Synchronize
{
  using System.Collections.Generic;

  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Entities;
  using Sitecore.MediaFramework.Synchronize.Fallback;
  using Sitecore.MediaFramework.Synchronize.References;

  public interface IItemSynchronizer : IDatabaseFallback, IMediaServiceEntityCreator
  {
    List<IReferenceSynchronizer> References { get; }

    Item SyncItem(object entity, Item accountItem);

    Item UpdateItem(object entity, Item accountItem, Item item);

    Item GetRootItem(object entity, Item accountItem);

    bool NeedUpdate(object entity, Item accountItem, MediaServiceSearchResult searchResult);

    MediaServiceSearchResult GetSearchResult(object entity, Item accountItem);

    MediaServiceEntityData GetMediaData(object entity);
  }
}