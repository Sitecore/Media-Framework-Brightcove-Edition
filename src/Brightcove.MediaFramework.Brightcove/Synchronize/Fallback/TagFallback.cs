using Brightcove.MediaFramework.Brightcove.Entities;
using Brightcove.MediaFramework.Brightcove.Indexing.Entities;
using Sitecore.Data;
using Sitecore.Data.Items;

using Sitecore.MediaFramework.Entities;

namespace Brightcove.MediaFramework.Brightcove.Synchronize.Fallback
{
    public class TagFallback : AssetFallback<TagSearchResult>
    {
        protected override Item GetItem(object entity, Item accountItem)
        {
            Tag tag = (Tag)entity;
            return accountItem.Axes.SelectSingleItem(string.Format("./Tags//*[@@templateid='{0}' and @@name='{1}']", (object)TemplateIDs.Tag, (object)ItemUtil.ProposeValidItemName(tag.Name)));
        }

        protected override MediaServiceSearchResult GetSearchResult(Item item)
        {
            TagSearchResult tagSearchResult = new TagSearchResult();
            tagSearchResult.Name = item[FieldIDs.Tag.Name];
            return (MediaServiceSearchResult)tagSearchResult;
        }
    }

}
