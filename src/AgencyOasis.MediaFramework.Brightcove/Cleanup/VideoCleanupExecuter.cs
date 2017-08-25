using System;
using System.Collections.Generic;
using System.Linq;
using AgencyOasis.MediaFramework.Brightcove.Entities;
using AgencyOasis.MediaFramework.Brightcove.Indexing.Entities;
using Sitecore.Data.Items;
using Sitecore.MediaFramework.Cleanup;

namespace AgencyOasis.MediaFramework.Brightcove.Cleanup
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
        AgencyOasis.MediaFramework.Brightcove.Entities.ItemState? itemState = i.ItemState;
        if (itemState.GetValueOrDefault() == AgencyOasis.MediaFramework.Brightcove.Entities.ItemState.ACTIVE)
          return itemState.HasValue;
        return false;
      })));
    }
  }
}
