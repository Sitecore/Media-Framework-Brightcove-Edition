namespace Sitecore.MediaFramework.Synchronize.References
{
  using Sitecore.Data.Items;

  public interface IReferenceSynchronizer
  {
    Item SyncReference(object entity, Item accountItem, Item item);
  }
}