using Brightcove.MediaFramework.Brightcove.Entities;
using Brightcove.MediaFramework.Brightcove.Indexing.Entities;
using Sitecore.MediaFramework.Cleanup;

namespace Brightcove.MediaFramework.Brightcove.Cleanup
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
