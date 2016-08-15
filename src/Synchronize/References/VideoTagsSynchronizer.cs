
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

  public class VideoTagsSynchronizer : IdReferenceSynchronizer<Video>
  {
    protected override List<ID> GetReference(Video entity, Item accountItem)
    {
      if (entity.Tags == null || entity.Tags.Count == 0)
      {
        return new List<ID>(0);
      }

      var expression = ContentSearchUtil.GetAncestorFilter<TagSearchResult>(accountItem, TemplateIDs.Tag);
      var nameExp = entity.Tags.Aggregate(PredicateBuilder.False<TagSearchResult>(), (current, tmp) => current.Or(i => i.TagName == tmp));

      List<TagSearchResult> searchResults = ContentSearchUtil.FindAll(Constants.IndexName, expression.And(nameExp));

      //fallback
      if (searchResults.Count < entity.Tags.Count)
      {
        IItemSynchronizer synchronizer = MediaFrameworkContext.GetItemSynchronizer(typeof(Tag));
        if (synchronizer != null)
        {
          foreach (string tagName in entity.Tags)
          {
            if (searchResults.Any(i => i.Name == tagName))
            {
              continue;
            }

            var tagIndex = synchronizer.Fallback(new Tag {Name = tagName}, accountItem) as TagSearchResult;
            if (tagIndex != null)
            {
              searchResults.Add(tagIndex);
            }
          }
        }
      }

      return searchResults.Select(i => i.ItemId).ToList();
    }
  }
}