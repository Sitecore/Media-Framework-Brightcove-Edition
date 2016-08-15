namespace Sitecore.MediaFramework.Brightcove.Synchronize
{
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Brightcove.Entities;
  using Sitecore.MediaFramework.Brightcove.Indexing.Entities;
  using Sitecore.MediaFramework.Entities;
  using Integration.Common.Utils;

  /// <summary>
  /// Play List Synchronizer
  /// </summary>
  public class PlayListSynchronizer : AssetSynchronizer
  {
    /// <summary>
    /// Update Item
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="accountItem"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public override Item UpdateItem(object entity, Item accountItem, Item item)
    {
      var playList = (PlayList)entity;
      using (new EditContext(item))
      {
        item.Name = ItemUtil.ProposeValidItemName(playList.Name);

        item[FieldIDs.MediaElement.Name] = playList.Name;
        item[FieldIDs.PlayerList.PlaylistType] = playList.PlaylistType;
        item[FieldIDs.MediaElement.Id] = playList.Id;
        item[FieldIDs.MediaElement.ShortDescription] = playList.ShortDescription;
        item[FieldIDs.MediaElement.ThumbnailUrl] = playList.ThumbnailUrl;
        item[FieldIDs.MediaElement.ReferenceId] = playList.ReferenceId;
      }

      return item;
    }

    /// <summary>
    /// Need Update
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="accountItem"></param>
    /// <param name="searchResult"></param>
    /// <returns></returns>
    public override bool NeedUpdate(object entity, Item accountItem, MediaServiceSearchResult searchResult)
    {
      var playList = (PlayList)entity;
      var playListIndex = (PlaylistSearchResult)searchResult;

      return
        (!(StringUtil.EqualsIgnoreNullEmpty(playList.Name, playListIndex.AssetName)
           && StringUtil.EqualsIgnoreNullEmpty(playList.PlaylistType, playListIndex.PlaylistType)
           && StringUtil.EqualsIgnoreNullEmpty(playList.ReferenceId, playListIndex.ReferenceId)
           && StringUtil.EqualsIgnoreNullEmpty(playList.ThumbnailUrl, playListIndex.ThumbnailUrl)
           && StringUtil.EqualsIgnoreNullEmpty(playList.ShortDescription, playListIndex.ShortDescription)));
    }

    /// <summary>
    /// Get Index
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="accountItem"></param>
    /// <returns></returns>
    public override MediaServiceSearchResult GetSearchResult(object entity, Item accountItem)
    {
      var playlist = (PlayList)entity;

      return base.GetSearchResult<PlaylistSearchResult>(Constants.IndexName, accountItem, i => (i.TemplateId == TemplateIDs.Playlist) && i.Id == playlist.Id);
    }

    public override MediaServiceEntityData GetMediaData(object entity)
    {
      var mediaData = base.GetMediaData(entity);
      
      mediaData.TemplateId = TemplateIDs.Playlist;

      return mediaData;
    }
  }
}