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
    public class PlaylistVideoSynchronizer : IdReferenceSynchronizer<PlayList>
    {
        protected override List<ID> GetReference(PlayList entity, Item accountItem)
        {
            if (entity.VideoIds == null || entity.VideoIds.Count == 0)
                return new List<ID>(0);
            Expression<Func<VideoSearchResult, bool>> ancestorFilter = ContentSearchUtil.GetAncestorFilter<VideoSearchResult>(accountItem, TemplateIDs.Video);
            Expression<Func<VideoSearchResult, bool>> second = Enumerable.Aggregate<string, Expression<Func<VideoSearchResult, bool>>>((IEnumerable<string>)entity.VideoIds, PredicateBuilder.False<VideoSearchResult>(), (Func<Expression<Func<VideoSearchResult, bool>>, string, Expression<Func<VideoSearchResult, bool>>>)((current, tmp) => PredicateBuilder.Or<VideoSearchResult>(current, (Expression<Func<VideoSearchResult, bool>>)(i => i.Id == tmp))));
            List<VideoSearchResult> all = ContentSearchUtil.FindAll<VideoSearchResult>(Configuration.Settings.IndexName, PredicateBuilder.And<VideoSearchResult>(ancestorFilter, second));
            if (all.Count < entity.VideoIds.Count)
            {
                IItemSynchronizer itemSynchronizer1 = MediaFrameworkContext.GetItemSynchronizer(typeof(Video));
                if (itemSynchronizer1 != null)
                {
                    foreach (string str in entity.VideoIds)
                    {
                        string videoId = str;
                        if (!Enumerable.Any<VideoSearchResult>((IEnumerable<VideoSearchResult>)all, (Func<VideoSearchResult, bool>)(i => i.Id == videoId)))
                        {
                            IItemSynchronizer itemSynchronizer2 = itemSynchronizer1;
                            Video video1 = new Video();
                            video1.Id = videoId;
                            Video video2 = video1;
                            Item accountItem1 = accountItem;
                            VideoSearchResult videoSearchResult = itemSynchronizer2.Fallback((object)video2, accountItem1) as VideoSearchResult;
                            if (videoSearchResult != null)
                                all.Add(videoSearchResult);
                        }
                    }
                }
            }
            return Enumerable.ToList<ID>(Enumerable.Select<VideoSearchResult, ID>((IEnumerable<VideoSearchResult>)all, (Func<VideoSearchResult, ID>)(i => i.ItemId)));
        }
    }
}
