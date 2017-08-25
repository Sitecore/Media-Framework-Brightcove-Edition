using System;
using System.Linq.Expressions;
using AgencyOasis.MediaFramework.Brightcove.Entities;
using Sitecore.Data.Items;
using Sitecore.MediaFramework.Brightcove;
using Sitecore.MediaFramework.Brightcove.Indexing.Entities;
using Sitecore.MediaFramework.Brightcove.Synchronize;
using Sitecore.MediaFramework.Entities;

namespace AgencyOasis.MediaFramework.Brightcove.Synchronize
{
    public class PlayListSynchronizer : AssetSynchronizer
    {
        public override Item UpdateItem(object entity, Item accountItem, Item item)
        {
            PlayList playList = (PlayList)entity;
            using (new EditContext(item))
            {
                item.Name = ItemUtil.ProposeValidItemName(playList.Name);
                item[Sitecore.MediaFramework.Brightcove.FieldIDs.MediaElement.Name] = playList.Name;
                item[Sitecore.MediaFramework.Brightcove.FieldIDs.PlayerList.PlaylistType] = playList.PlaylistType;
                item[Sitecore.MediaFramework.Brightcove.FieldIDs.MediaElement.Id] = playList.Id;
                item[Sitecore.MediaFramework.Brightcove.FieldIDs.MediaElement.ShortDescription] = playList.ShortDescription;
                item[Sitecore.MediaFramework.Brightcove.FieldIDs.MediaElement.ReferenceId] = playList.ReferenceId;
                if (playList.PlayListSearch != null)
                {
                    item[Sitecore.MediaFramework.Brightcove.FieldIDs.PlayerList.TagInclusion] =
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

        public override bool NeedUpdate(object entity, Item accountItem, MediaServiceSearchResult searchResult)
        {
            PlayList playList = (PlayList)entity;
            PlaylistSearchResult playlistSearchResult = (PlaylistSearchResult)searchResult;
            if (Sitecore.Integration.Common.Utils.StringUtil.EqualsIgnoreNullEmpty(playList.Name, playlistSearchResult.AssetName) && Sitecore.Integration.Common.Utils.StringUtil.EqualsIgnoreNullEmpty(playList.PlaylistType, playlistSearchResult.PlaylistType) && (Sitecore.Integration.Common.Utils.StringUtil.EqualsIgnoreNullEmpty(playList.ReferenceId, playlistSearchResult.ReferenceId)))
                return !Sitecore.Integration.Common.Utils.StringUtil.EqualsIgnoreNullEmpty(playList.ShortDescription, playlistSearchResult.ShortDescription);
            return true;
        }

        public override MediaServiceSearchResult GetSearchResult(object entity, Item accountItem)
        {
            PlayList playlist = (PlayList)entity;
            return (MediaServiceSearchResult)this.GetSearchResult<PlaylistSearchResult>(Sitecore.MediaFramework.Brightcove.Constants.IndexName, accountItem, (Expression<Func<PlaylistSearchResult, bool>>)(i => i.TemplateId == TemplateIDs.Playlist && i.Id == playlist.Id));
        }

        public override MediaServiceEntityData GetMediaData(object entity)
        {
            MediaServiceEntityData mediaData = base.GetMediaData(entity);
            mediaData.TemplateId = TemplateIDs.Playlist;
            return mediaData;
        }
    }
}
