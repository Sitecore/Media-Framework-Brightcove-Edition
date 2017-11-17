namespace Sitecore.MediaFramework.Synchronize.Fallback
{
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Entities;

  public interface IDatabaseFallback
  {
    MediaServiceSearchResult Fallback(object entity, Item accountItem);
  }
}
