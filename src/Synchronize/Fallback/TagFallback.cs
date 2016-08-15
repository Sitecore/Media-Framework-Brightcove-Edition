namespace Sitecore.MediaFramework.Brightcove.Synchronize.Fallback
{
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Brightcove.Entities;
  using Sitecore.MediaFramework.Brightcove.Indexing.Entities;
  using Sitecore.MediaFramework.Entities;
  using Sitecore.MediaFramework.Synchronize.Fallback;

  public class TagFallback : DatabaseFallbackBase
  {
    protected override Item GetItem(object entity, Item accountItem)
    {
      var tag = (Tag)entity;
      return accountItem.Axes.SelectSingleItem(string.Format("./Tags//*[@@templateid='{0}' and @@name='{1}']", TemplateIDs.Tag, ItemUtil.ProposeValidItemName(tag.Name)));
    }

    protected override MediaServiceSearchResult GetSearchResult(Item item)
    {
      return new TagSearchResult
        {
          Name = item[FieldIDs.Tag.Name]
        };
    }
  }
}