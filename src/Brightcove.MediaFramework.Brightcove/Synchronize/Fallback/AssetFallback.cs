using Brightcove.MediaFramework.Brightcove.Indexing.Entities;
using Sitecore.Data.Items;
using Sitecore.MediaFramework.Entities;
using Sitecore.MediaFramework.Synchronize.Fallback;
using System;

namespace Brightcove.MediaFramework.Brightcove.Synchronize.Fallback
{
    public abstract class AssetFallback<T> : DatabaseFallbackBase where T : AssetSearchResult, new()
    {
        protected override MediaServiceSearchResult GetSearchResult(Item item)
        {
            T instance = Activator.CreateInstance<T>();
            instance.Id = item[FieldIDs.MediaElement.Id];
            instance.AssetName = item[FieldIDs.MediaElement.Name];
            instance.ReferenceId = item[FieldIDs.MediaElement.ReferenceId];
            instance.ThumbnailUrl = item[FieldIDs.MediaElement.ThumbnailUrl];
            instance.ShortDescription = item[FieldIDs.MediaElement.ShortDescription];
            return (MediaServiceSearchResult)instance;
        }
    }
}
