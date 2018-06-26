using Brightcove.MediaFramework.Brightcove.Entities;
using Sitecore.Data.Items;

using Sitecore.MediaFramework.Entities;
using Sitecore.MediaFramework.Export;
using Sitecore.MediaFramework.Synchronize;

namespace Brightcove.MediaFramework.Brightcove.Synchronize
{
  public abstract class AssetSynchronizer : SynchronizerBase
  {
    public override Item SyncItem(object entity, Item accountItem)
    {
      Asset asset = (Asset) entity;
      if (ExportQueueManager.IsExist(accountItem, FieldIDs.MediaElement.Id, asset.Id))
        return (Item) null;
      return base.SyncItem(entity, accountItem);
    }

    public override Item GetRootItem(object entity, Item accountItem)
    {
      return accountItem.Children["Media Content"];
    }

    public override MediaServiceEntityData GetMediaData(object entity)
    {
      Asset asset = (Asset) entity;
      return new MediaServiceEntityData()
      {
        EntityId = asset.Id,
        EntityName = asset.Name
      };
    }
  }
}
