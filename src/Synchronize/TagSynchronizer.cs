namespace Sitecore.MediaFramework.Brightcove.Synchronize
{
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Brightcove.Entities;
  using Sitecore.MediaFramework.Brightcove.Indexing.Entities;
  using Sitecore.MediaFramework.Entities;
  using Sitecore.MediaFramework.Synchronize;

  public class TagSynchronizer : SynchronizerBase
  {
    public override Item UpdateItem(object entity, Item accountItem, Item item)
    {
      var tag = (Tag)entity;

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
      var tag = (Tag)entity;

      return base.GetSearchResult<TagSearchResult>(Constants.IndexName, accountItem, i => i.TemplateId == TemplateIDs.Tag && i.Name == ItemUtil.ProposeValidItemName(tag.Name));
    }

    public override MediaServiceEntityData GetMediaData(object entity)
    {
      var tag = (Tag)entity;

      return new MediaServiceEntityData { EntityId = tag.Name, EntityName = tag.Name, TemplateId = TemplateIDs.Tag };
    }
  }
}