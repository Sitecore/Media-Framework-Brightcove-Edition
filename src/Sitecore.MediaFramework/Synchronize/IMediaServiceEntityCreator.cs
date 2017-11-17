namespace Sitecore.MediaFramework.Synchronize
{
  using Sitecore.Data.Items;

  public interface IMediaServiceEntityCreator                        
  {
    object CreateEntity(Item item);
  }
}