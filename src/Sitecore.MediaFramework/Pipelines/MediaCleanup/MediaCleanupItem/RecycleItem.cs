namespace Sitecore.MediaFramework.Pipelines.MediaCleanup.MediaCleanupItem
{
  using Sitecore.Data.Items;

  public class RecycleItem : DeleteItem
  {
    protected override void RemoveItem(Item item)
    {
      item.Recycle();
    }
  }
}