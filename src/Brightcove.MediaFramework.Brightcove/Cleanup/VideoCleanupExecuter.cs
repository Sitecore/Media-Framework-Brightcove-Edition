using System;
using System.Collections.Generic;
using System.Linq;
using Brightcove.MediaFramework.Brightcove.Entities;
using Brightcove.MediaFramework.Brightcove.Indexing.Entities;
using Sitecore.Data.Items;
using Sitecore.MediaFramework.Cleanup;

namespace Brightcove.MediaFramework.Brightcove.Cleanup
{
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
      List<Video> serviceData = base.GetServiceData(accountItem);
      if (serviceData == null)
        return (List<Video>) null;
      return Enumerable.ToList<Video>(Enumerable.Where<Video>((IEnumerable<Video>) serviceData, (Func<Video, bool>) (i =>
      {
        Entities.ItemState? itemState = i.ItemState;
        if (itemState.GetValueOrDefault() == Entities.ItemState.ACTIVE)
          return itemState.HasValue;
        return false;
      })));
    }
  }
}
