using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using Brightcove.MediaFramework.Brightcove.Entities;
using Sitecore.Data.Items;

using Sitecore.MediaFramework.Entities;
using VideoSearchResult = Brightcove.MediaFramework.Brightcove.Indexing.Entities.VideoSearchResult;

namespace Brightcove.MediaFramework.Brightcove.Synchronize
{
    public class FakePlayerSynchronizer : AssetSynchronizer
    {
        public override object CreateEntity(Item item)
        {
            return (object)new Player()
            {
                Id = item[FieldIDs.Player.Id],
                Name = item.Name
            };
        }

        public override MediaServiceEntityData GetMediaData(object entity)
        {
            Player player = (Player)entity;
            return new MediaServiceEntityData()
            {
                EntityId = player.Id,
                EntityName = player.Name,
                TemplateId = TemplateIDs.Player
            };
        }

        public override MediaServiceSearchResult Fallback(object entity, Item accountItem)
        {
            return (MediaServiceSearchResult)null;
        }

        public override Item SyncItem(object entity, Item accountItem)
        {
            return (Item)null;
        }

        public override Item UpdateItem(object entity, Item accountItem, Item item)
        {
            return (Item)null;
        }

        public override Item GetRootItem(object entity, Item accountItem)
        {
            return (Item)null;
        }

        public override bool NeedUpdate(object entity, Item accountItem, MediaServiceSearchResult searchResult)
        {
            return false;
        }

        public override MediaServiceSearchResult GetSearchResult(object entity, Item accountItem)
        {
            return (MediaServiceSearchResult)null;
        }
    }
}
