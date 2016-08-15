namespace Sitecore.MediaFramework.Brightcove.Cleanup
{
  using System.Collections.Generic;
  using System.Linq;

  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Brightcove.Entities;
  using Sitecore.MediaFramework.Brightcove.Indexing.Entities;
  using Sitecore.MediaFramework.Cleanup;

  public class VideoCleanupExecuter : CleanupExecuterBase<Video, VideoSearchResult>
  {
    protected override string GetEntityId(Video entity)
    {
      return entity.Id;
    }

    protected override string GetSearchResultId(VideoSearchResult searchResult)
    {
      return searchResult.Id;
    }

    protected override List<Video> GetServiceData(Item accountItem)
    {
      var data = base.GetServiceData(accountItem);
      return data != null ? data.Where(i => i.ItemState == Entities.ItemState.ACTIVE).ToList() : null;
    }
  }
}