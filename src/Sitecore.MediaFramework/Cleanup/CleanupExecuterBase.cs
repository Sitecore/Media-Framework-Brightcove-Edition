
namespace Sitecore.MediaFramework.Cleanup
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Integration.Common.Utils;
  using Sitecore.MediaFramework.Diagnostics;
  using Sitecore.MediaFramework.Entities;
  using Sitecore.MediaFramework.Import;

  public abstract class CleanupExecuterBase<TEntity,TSearchResult> : ICleanupExecuter
    where TSearchResult : MediaServiceSearchResult, new()
  {
    public string ImportName { get; set; }

    public string IndexName { get; set; }
    
    public List<ID> Templates { get; protected set; }

    protected CleanupExecuterBase()
    {
      this.Templates = new List<ID>();
    }

    public void AddTemplate(string id)
    {
      if (ID.IsID(id))
      {
        this.Templates.Add(new ID(id));
      }
    }

    public virtual IEnumerable<Item> GetScopeItems(Item accountItem)
    {
      var serviceData = this.GetServiceData(accountItem);
      if (serviceData == null)
      {
        LogHelper.Debug(string.Format("Service data({0}) is null. Account Id:{1}", typeof(TEntity), accountItem.ID), this);

        yield break;
      }

      string[] serviceIds = serviceData.Select(this.GetEntityId).ToArray();

      LogHelper.Debug(string.Format("Entities({0}) ids from service:{1}", typeof(TEntity), string.Join("|", serviceIds)), this);

      var sitecoreData = this.GetSitecoreData(this.IndexName, accountItem);
      foreach (TSearchResult searchResult in sitecoreData)
      {
        string searchResultId = this.GetSearchResultId(searchResult);

        LogHelper.Debug(string.Format("Search result({0}) checking.SearchResultId:{1}", typeof(TSearchResult), searchResultId), this);

        if (string.IsNullOrEmpty(searchResultId) || Array.IndexOf(serviceIds, searchResultId) >= 0)
        {
          continue;
        }

        Item item = searchResult.GetItem();
        if (item != null)
        {
          LogHelper.Debug(string.Format("Search result({0}) id:{1} has been added to cleanup list", typeof(TSearchResult), searchResultId), this);

          yield return item;
        }
      }
    }

    protected abstract string GetEntityId(TEntity entity);

    protected abstract string GetSearchResultId(TSearchResult searchResult);

    [CanBeNull]
    protected virtual List<TEntity> GetServiceData(Item accountItem)
    {
      return ImportManager.ImportList<TEntity>(this.ImportName, accountItem);
    }

    protected virtual List<TSearchResult> GetSitecoreData(string indexName, Item accountItem)
    {
      var expression = ContentSearchUtil.GetAncestorFilter<TSearchResult>(accountItem, this.Templates);

      return ContentSearchUtil.FindAll(indexName, expression);
    }
  }
}