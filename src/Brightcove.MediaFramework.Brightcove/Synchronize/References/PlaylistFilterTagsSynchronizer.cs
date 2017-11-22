using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Brightcove.MediaFramework.Brightcove.Entities;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Integration.Common.Utils;
using Sitecore.MediaFramework;

using Sitecore.MediaFramework.Synchronize;
using Sitecore.MediaFramework.Synchronize.References;
using Brightcove.MediaFramework.Brightcove.Indexing.Entities;

namespace Brightcove.MediaFramework.Brightcove.Synchronize.References
{
    public class PlaylistFilterTagsSynchronizer : IdReferenceSynchronizer<PlayList>
    {
        protected override List<ID> GetReference(PlayList entity, Item accountItem)
        {
            if (entity.PlayListSearch == null || entity.PlayListSearch.FilterTags == null || entity.PlayListSearch.FilterTags.Count == 0)
                return new List<ID>(0);
            Expression<Func<TagSearchResult, bool>> ancestorFilter = ContentSearchUtil.GetAncestorFilter<TagSearchResult>(accountItem, TemplateIDs.Tag);
            Expression<Func<TagSearchResult, bool>> second = Enumerable.Aggregate<string, Expression<Func<TagSearchResult, bool>>>((IEnumerable<string>)entity.PlayListSearch.FilterTags, PredicateBuilder.False<TagSearchResult>(), (Func<Expression<Func<TagSearchResult, bool>>, string, Expression<Func<TagSearchResult, bool>>>)((current, tmp) => PredicateBuilder.Or<TagSearchResult>(current, (Expression<Func<TagSearchResult, bool>>)(i => i.TagName == tmp))));
            List<TagSearchResult> all = ContentSearchUtil.FindAll<TagSearchResult>(Configuration.Settings.IndexName, PredicateBuilder.And<TagSearchResult>(ancestorFilter, second));
            if (all.Count < entity.PlayListSearch.FilterTags.Count)
            {
                IItemSynchronizer itemSynchronizer = MediaFrameworkContext.GetItemSynchronizer(typeof(Entities.Tag));
                if (itemSynchronizer != null)
                {
                    foreach (string str in entity.PlayListSearch.FilterTags)
                    {
                        string tagName = str;
                        if (!Enumerable.Any<TagSearchResult>((IEnumerable<TagSearchResult>)all, (Func<TagSearchResult, bool>)(i => i.Name == tagName)))
                        {
                            TagSearchResult tagSearchResult = itemSynchronizer.Fallback((object)new Entities.Tag()
                            {
                                Name = tagName
                            }, accountItem) as TagSearchResult;
                            if (tagSearchResult != null)
                                all.Add(tagSearchResult);
                        }
                    }
                }
            }
            return Enumerable.ToList<ID>(Enumerable.Select<TagSearchResult, ID>((IEnumerable<TagSearchResult>)all, (Func<TagSearchResult, ID>)(i => i.ItemId)));
        }
    }
}
