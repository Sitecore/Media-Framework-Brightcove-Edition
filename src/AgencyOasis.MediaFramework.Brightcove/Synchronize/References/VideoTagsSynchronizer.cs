using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AgencyOasis.MediaFramework.Brightcove.Entities;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Integration.Common.Utils;
using Sitecore.MediaFramework;
using Sitecore.MediaFramework.Brightcove.Indexing.Entities;
using Sitecore.MediaFramework.Synchronize;
using Sitecore.MediaFramework.Synchronize.References;

namespace AgencyOasis.MediaFramework.Brightcove.Synchronize.References
{
  public class VideoTagsSynchronizer : IdReferenceSynchronizer<Video>
  {
    protected override List<ID> GetReference(Video entity, Item accountItem)
    {
      if (entity.Tags == null || entity.Tags.Count == 0)
        return new List<ID>(0);
      Expression<Func<TagSearchResult, bool>> ancestorFilter = ContentSearchUtil.GetAncestorFilter<TagSearchResult>(accountItem, Sitecore.MediaFramework.Brightcove.TemplateIDs.Tag);
      Expression<Func<TagSearchResult, bool>> second = Enumerable.Aggregate<string, Expression<Func<TagSearchResult, bool>>>((IEnumerable<string>) entity.Tags, PredicateBuilder.False<TagSearchResult>(), (Func<Expression<Func<TagSearchResult, bool>>, string, Expression<Func<TagSearchResult, bool>>>) ((current, tmp) => PredicateBuilder.Or<TagSearchResult>(current, (Expression<Func<TagSearchResult, bool>>) (i => i.TagName == tmp))));
      List<TagSearchResult> all = ContentSearchUtil.FindAll<TagSearchResult>(Sitecore.MediaFramework.Brightcove.Constants.IndexName, PredicateBuilder.And<TagSearchResult>(ancestorFilter, second));
      if (all.Count < entity.Tags.Count)
      {
        IItemSynchronizer itemSynchronizer = MediaFrameworkContext.GetItemSynchronizer(typeof (Sitecore.MediaFramework.Brightcove.Entities.Tag));
        if (itemSynchronizer != null)
        {
          foreach (string str in entity.Tags)
          {
            string tagName = str;
            if (!Enumerable.Any<TagSearchResult>((IEnumerable<TagSearchResult>) all, (Func<TagSearchResult, bool>) (i => i.Name == tagName)))
            {
              TagSearchResult tagSearchResult = itemSynchronizer.Fallback((object) new Sitecore.MediaFramework.Brightcove.Entities.Tag()
              {
                Name = tagName
              }, accountItem) as TagSearchResult;
              if (tagSearchResult != null)
                all.Add(tagSearchResult);
            }
          }
        }
      }
      return Enumerable.ToList<ID>(Enumerable.Select<TagSearchResult, ID>((IEnumerable<TagSearchResult>) all, (Func<TagSearchResult, ID>) (i => i.ItemId)));
    }
  }
}
