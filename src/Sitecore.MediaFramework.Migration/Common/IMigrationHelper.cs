namespace Sitecore.MediaFramework.Migration.Common
{
  using System.Collections.Specialized;

  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Players;
  using Sitecore.MediaFramework.Synchronize;

  public interface IMigrationHelper
  {
    Item GetMediaItem(Database database, NameValueCollection collection);

    Item GetPlayerItem(Database database, NameValueCollection collection);

    Item GetAccountItem(Item mediaItem);

    IPlayerMarkupGenerator GetPlayerMarkupGenerator(Item mediaItem);

    IItemSynchronizer GetItemSynchronizer(Item mediaItem);

    object CreateEntity(Item mediaItem, IItemSynchronizer synchronizer = null);

    ID GetNewItemId(Item mediaItem);

    Item RecreateItem(Item mediaItem, bool remove);

    void UpdateReferrers(Item oldItem, Item newItem);
  }
}