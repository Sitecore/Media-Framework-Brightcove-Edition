using AgencyOasis.MediaFramework.Brightcove.Entities;
using Sitecore.Data.Items;
using Sitecore.MediaFramework.Brightcove;
using Sitecore.MediaFramework.Entities;
using Sitecore.MediaFramework.Export;
using Sitecore.MediaFramework.Synchronize;

namespace AgencyOasis.MediaFramework.Brightcove.Synchronize
{
  public abstract class AssetSynchronizer : SynchronizerBase
  {
    public override Item SyncItem(object entity, Item accountItem)
    {
      Asset asset = (Asset) entity;
      if (ExportQueueManager.IsExist(accountItem, Sitecore.MediaFramework.Brightcove.FieldIDs.MediaElement.Id, asset.Id))
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
