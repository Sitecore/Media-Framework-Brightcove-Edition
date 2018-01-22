using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using Brightcove.MediaFramework.Brightcove.Entities;
using Sitecore.Data.Items;

using Sitecore.MediaFramework.Entities;
using VideoSearchResult = Brightcove.MediaFramework.Brightcove.Indexing.Entities.VideoSearchResult;
using Brightcove.MediaFramework.Brightcove.Indexing.Entities;

namespace Brightcove.MediaFramework.Brightcove.Synchronize
{
    public class TagSynchronizer : AssetSynchronizer
    {
        public override Item UpdateItem(object entity, Item accountItem, Item item)
        {
            Tag tag = (Tag)entity;
            using (new EditContext(item))
            {
                item[Sitecore.FieldIDs.DisplayName] = tag.Name;
                item[FieldIDs.Tag.Name] = tag.Name;
            }
            return item;
        }

        public override Item GetRootItem(object entity, Item accountItem)
        {
            return accountItem.Children["Tags"];
        }

        public override bool NeedUpdate(object entity, Item accountItem, MediaServiceSearchResult searchResult)
        {
            return false;
        }

        public override MediaServiceSearchResult GetSearchResult(object entity, Item accountItem)
        {
            Tag tag = (Tag)entity;
            return (MediaServiceSearchResult)this.GetSearchResult<TagSearchResult>(Configuration.Settings.IndexName, accountItem, (Expression<Func<TagSearchResult, bool>>)(i => i.TemplateId == TemplateIDs.Tag && i.TagName == tag.Name));
        }

        public override MediaServiceEntityData GetMediaData(object entity)
        {
            Tag tag = (Tag)entity;
            return new MediaServiceEntityData()
            {
                EntityId = tag.Name,
                EntityName = tag.Name,
                TemplateId = TemplateIDs.Tag
            };
        }
    }
}
