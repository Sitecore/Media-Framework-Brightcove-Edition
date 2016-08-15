namespace Sitecore.MediaFramework.Brightcove.Synchronize.Fallback
{
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Brightcove.Indexing.Entities;
  using Sitecore.MediaFramework.Entities;
  using Sitecore.MediaFramework.Synchronize.Fallback;

  public abstract class AssetFallback<T> : DatabaseFallbackBase
    where T : AssetSearchResult, new()
  {
    protected override MediaServiceSearchResult GetSearchResult(Item item)
    {
      return new T
        {
          Id = item[FieldIDs.MediaElement.Id],
          AssetName = item[FieldIDs.MediaElement.Name],
          ReferenceId = item[FieldIDs.MediaElement.ReferenceId],
          ThumbnailUrl = item[FieldIDs.MediaElement.ThumbnailUrl],
          ShortDescription = item[FieldIDs.MediaElement.ShortDescription]
        };
    }
  }
}