namespace Sitecore.MediaFramework.Brightcove.Synchronize.EntityCreators
{
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Brightcove.Entities;
  using Sitecore.MediaFramework.Synchronize;

  public class TagEntityCreator : IMediaServiceEntityCreator
  {
    public virtual object CreateEntity(Item item)
    {
      return new Tag { Name = item[FieldIDs.Tag.Name] };
    }
  }
}