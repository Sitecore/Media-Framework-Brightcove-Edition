
namespace Sitecore.MediaFramework.Synchronize
{
  using System;
  using System.Collections.Generic;
  using System.Linq.Expressions;
  using System.Xml;

  using Sitecore.Configuration;
  using Sitecore.ContentSearch.Linq.Utilities;
  using Sitecore.Data.Items;
  using Sitecore.Integration.Common.Utils;
  using Sitecore.MediaFramework.Entities;
  using Sitecore.MediaFramework.Pipelines.MediaSyncImport.MediaSyncItemImport;
  using Sitecore.MediaFramework.Synchronize.Fallback;
  using Sitecore.MediaFramework.Synchronize.References;

  public abstract class SynchronizerBase : IItemSynchronizer
  {
    protected SynchronizerBase()
    {
      this.References = new List<IReferenceSynchronizer>();
    }

    public IDatabaseFallback DatabaseFallback { get; set; }
    public IMediaServiceEntityCreator EntityCreator { get; set; }

    public List<IReferenceSynchronizer> References { get; protected set; }

    public void AddReference(XmlNode node)
    {
      var obj = Factory.CreateObject<IReferenceSynchronizer>(node);
      if (obj != null)
      {
        this.References.Add(obj);
      }
    }

    public virtual Item SyncItem(object entity, Item accountItem)
    {
      var args = new MediaSyncItemImportArgs
        {
          Entity = entity,
          AccountItem = accountItem,
          Synchronizer = this,
          SyncAllowActivity = SyncAllowActivity.All
        };
      MediaSyncItemImportPipeline.Run(args);

      return args.Item;
    }

    public abstract Item UpdateItem(object entity, Item accountItem, Item item);

    public abstract Item GetRootItem(object entity, Item accountItem);

    public abstract bool NeedUpdate(object entity, Item accountItem, MediaServiceSearchResult searchResult);

    public abstract MediaServiceSearchResult GetSearchResult(object entity, Item accountItem);

    public abstract MediaServiceEntityData GetMediaData(object entity);

    public virtual MediaServiceSearchResult Fallback(object entity, Item accountItem)
    {
      return this.DatabaseFallback != null ? this.DatabaseFallback.Fallback(entity, accountItem) : null;
    }

    public virtual object CreateEntity(Item item)
    {
      return this.EntityCreator.CreateEntity(item);
    }

    protected virtual TSearchResult GetSearchResult<TSearchResult>(string indexName, Item accountItem, Expression<Func<TSearchResult, bool>> selector) where TSearchResult : MediaServiceSearchResult, new()
    {
      Expression<Func<TSearchResult, bool>> predicate = ContentSearchUtil.GetAncestorFilter<TSearchResult>(accountItem);

      return ContentSearchUtil.FindOne<TSearchResult>(indexName, predicate.And(selector));
    }
  }
}