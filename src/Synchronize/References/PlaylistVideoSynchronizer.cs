namespace Sitecore.MediaFramework.Brightcove.Synchronize.References
{
  using System.Collections.Generic;
  using System.Linq;

  using Sitecore.ContentSearch.Linq.Utilities;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Integration.Common.Utils;
  using Sitecore.MediaFramework.Brightcove.Entities;
  using Sitecore.MediaFramework.Brightcove.Indexing.Entities;
  using Sitecore.MediaFramework.Synchronize;
  using Sitecore.MediaFramework.Synchronize.References;

  public class PlaylistVideoSynchronizer : IdReferenceSynchronizer<PlayList>
  {
    protected override List<ID> GetReference(PlayList entity, Item accountItem)
    {
      if (entity.VideoIds == null || entity.VideoIds.Count == 0)
      {
        return new List<ID>(0);
      }

      var expression = ContentSearchUtil.GetAncestorFilter<VideoSearchResult>(accountItem, TemplateIDs.Video);
      var nameExp = entity.VideoIds.Aggregate(PredicateBuilder.False<VideoSearchResult>(), (current, tmp) => current.Or(i => i.Id == tmp));

      List<VideoSearchResult> searchResults = ContentSearchUtil.FindAll(Constants.IndexName, expression.And(nameExp));

      //fallback
      if (searchResults.Count < entity.VideoIds.Count)
      {
        IItemSynchronizer synchronizer = MediaFrameworkContext.GetItemSynchronizer(typeof(Video));
        if (synchronizer != null)
        {
          foreach (string videoId in entity.VideoIds)
          {
            if (searchResults.Any(i => i.Id == videoId))
            {
              continue;
            }

            var videoIndex = synchronizer.Fallback(new Video { Id = videoId }, accountItem) as VideoSearchResult;
            if (videoIndex != null)
            {
              searchResults.Add(videoIndex);
            }
          }
        }
      }

      return searchResults.Select(i => i.ItemId).ToList();
    }
  }
}