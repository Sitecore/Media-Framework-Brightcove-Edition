using Brightcove.MediaFramework.Brightcove.Entities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.MediaFramework.Brightcove;
using Sitecore.MediaFramework.Brightcove.Indexing.Entities;
using Sitecore.MediaFramework.Brightcove.Synchronize.Fallback;
using Sitecore.MediaFramework.Entities;

namespace Brightcove.MediaFramework.Brightcove.Synchronize.Fallback
{
  public class PlayListFallback : AssetFallback<PlaylistSearchResult>
  {
    protected override Item GetItem(object entity, Item accountItem)
    {
      PlayList playList = (PlayList) entity;
      return accountItem.Axes.SelectSingleItem(string.Format("./Media Content//*[@@templateid='{0}' and @id='{1}']", (object) TemplateIDs.Playlist, (object) playList.Id));
    }

    protected override MediaServiceSearchResult GetSearchResult(Item item)
    {
      PlaylistSearchResult playlistSearchResult = (PlaylistSearchResult) base.GetSearchResult(item);
      playlistSearchResult.PlaylistType = item[Sitecore.MediaFramework.Brightcove.FieldIDs.PlayerList.PlaylistType];
      playlistSearchResult.FilterTags = ID.ParseArray(item[Sitecore.MediaFramework.Brightcove.FieldIDs.PlayerList.FilterTags]);
      playlistSearchResult.VideoIds = ID.ParseArray(item[Sitecore.MediaFramework.Brightcove.FieldIDs.PlayerList.VideoIds]);
      return (MediaServiceSearchResult) playlistSearchResult;
    }
  }
}
