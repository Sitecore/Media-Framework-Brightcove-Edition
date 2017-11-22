using System;
using System.Linq.Expressions;
using Brightcove.MediaFramework.Brightcove.Entities;
using Sitecore.Data.Items;
using Sitecore.MediaFramework.Entities;
using Brightcove.MediaFramework.Brightcove.Indexing.Entities;

namespace Brightcove.MediaFramework.Brightcove.Synchronize
{
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
            PlayList playList = (PlayList)entity;
            using (new EditContext(item))
            {
                item.Name = ItemUtil.ProposeValidItemName(playList.Name);
                item[FieldIDs.MediaElement.Name] = playList.Name;
                item[FieldIDs.PlayerList.PlaylistType] = playList.PlaylistType;
                item[FieldIDs.MediaElement.Id] = playList.Id;
                item[FieldIDs.MediaElement.ShortDescription] = playList.ShortDescription;
                item[FieldIDs.MediaElement.ReferenceId] = playList.ReferenceId;
                if (playList.PlayListSearch != null)
                {
                    item[FieldIDs.PlayerList.TagInclusion] =
                        playList.PlayListSearch.TagInclusion.ToString();
                }
                item[FieldIDs.Playlist.CreationDate] = playList.CreationDate.HasValue ?
                playList.CreationDate.ToString() : string.Empty;
                item[FieldIDs.Playlist.LastModifiedDate] = playList.LastModifiedDate.HasValue ?
                playList.CreationDate.ToString() : string.Empty;
                item[FieldIDs.Playlist.Favorite] = playList.Favorite.HasValue ?
                    playList.Favorite.Value ? "1" : "0" : string.Empty;
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
            PlayList playList = (PlayList)entity;
            PlaylistSearchResult playlistSearchResult = (PlaylistSearchResult)searchResult;
            if (Sitecore.Integration.Common.Utils.StringUtil.EqualsIgnoreNullEmpty(playList.Name, playlistSearchResult.AssetName) && Sitecore.Integration.Common.Utils.StringUtil.EqualsIgnoreNullEmpty(playList.PlaylistType, playlistSearchResult.PlaylistType) && (Sitecore.Integration.Common.Utils.StringUtil.EqualsIgnoreNullEmpty(playList.ReferenceId, playlistSearchResult.ReferenceId)))
                return !Sitecore.Integration.Common.Utils.StringUtil.EqualsIgnoreNullEmpty(playList.ShortDescription, playlistSearchResult.ShortDescription);
            return true;
        }

        /// <summary>
        /// Get Index
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="accountItem"></param>
        /// <returns></returns>
        public override MediaServiceSearchResult GetSearchResult(object entity, Item accountItem)
        {
            PlayList playlist = (PlayList)entity;
            return (MediaServiceSearchResult)this.GetSearchResult<PlaylistSearchResult>(Configuration.Settings.IndexName, accountItem, (Expression<Func<PlaylistSearchResult, bool>>)(i => i.TemplateId == TemplateIDs.Playlist && i.Id == playlist.Id));
        }

        public override MediaServiceEntityData GetMediaData(object entity)
        {
            MediaServiceEntityData mediaData = base.GetMediaData(entity);
            mediaData.TemplateId = TemplateIDs.Playlist;
            return mediaData;
        }
    }
}
