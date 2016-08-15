
namespace Sitecore.MediaFramework.Brightcove.Synchronize.Fallback
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Brightcove.Entities;
  using Sitecore.MediaFramework.Brightcove.Indexing.Entities;
  using Sitecore.MediaFramework.Entities;

  public class PlayListFallback : AssetFallback<PlaylistSearchResult>
  {
    protected override Item GetItem(object entity, Item accountItem)
    {
      var playList = (PlayList)entity;
      return accountItem.Axes.SelectSingleItem(string.Format("./Media Content//*[@@templateid='{0}' and @id='{1}']", TemplateIDs.Playlist, playList.Id));
    }

    protected override MediaServiceSearchResult GetSearchResult(Item item)
    {
      var searchResult = (PlaylistSearchResult)base.GetSearchResult(item);
      
      searchResult.PlaylistType = item[FieldIDs.PlayerList.PlaylistType];
      searchResult.FilterTags = ID.ParseArray(item[FieldIDs.PlayerList.FilterTags]);
      searchResult.VideoIds = ID.ParseArray(item[FieldIDs.PlayerList.VideoIds]);

      return searchResult;
    }
  }
}