namespace Sitecore.MediaFramework.Brightcove.Cleanup
{
  using Sitecore.MediaFramework.Brightcove.Entities;
  using Sitecore.MediaFramework.Brightcove.Indexing.Entities;
  using Sitecore.MediaFramework.Cleanup;

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