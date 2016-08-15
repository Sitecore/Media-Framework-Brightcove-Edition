namespace Sitecore.MediaFramework.Brightcove.Synchronize
{
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Brightcove.Entities;
  using Sitecore.MediaFramework.Entities;
  using Sitecore.MediaFramework.Synchronize;

  public class FakePlayerSynchronizer : SynchronizerBase
  {
    public override object CreateEntity(Item item)
    {
      return new Player { Id = item[FieldIDs.Player.Id], Name = item.Name, };
    }

    public override MediaServiceEntityData GetMediaData(object entity)
    {
      var player = (Player)entity;

      return new MediaServiceEntityData
      {
        EntityId = player.Id,
        EntityName = player.Name,
        TemplateId = TemplateIDs.Player
      };
    }

    public override MediaServiceSearchResult Fallback(object entity, Item accountItem)
    {
      return null;
    }

    public override Item SyncItem(object entity, Item accountItem)
    {
      return null;
    }

    public override Item UpdateItem(object entity, Item accountItem, Item item)
    {
      return null;
    }

    public override Item GetRootItem(object entity, Item accountItem)
    {
      return null;
    }

    public override bool NeedUpdate(object entity, Item accountItem, MediaServiceSearchResult searchResult)
    {
      return false;
    }

    public override MediaServiceSearchResult GetSearchResult(object entity, Item accountItem)
    {
      return null;
    }
  }
}