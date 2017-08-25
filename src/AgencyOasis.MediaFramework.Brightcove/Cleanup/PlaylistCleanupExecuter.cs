using AgencyOasis.MediaFramework.Brightcove.Entities;
using Sitecore.MediaFramework.Brightcove.Indexing.Entities;
using Sitecore.MediaFramework.Cleanup;

namespace AgencyOasis.MediaFramework.Brightcove.Cleanup
{
  public class PlaylistCleanupExecuter : CleanupExecuterBase<PlayList, PlaylistSearchResult>
  {
    protected override string GetEntityId(PlayList entity)
    {
      return entity.Id;
    }

    protected override string GetSearchResultId(PlaylistSearchResult searchResult)
    {
      return searchResult.Id;
    }
  }
}
