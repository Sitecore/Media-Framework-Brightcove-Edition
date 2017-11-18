using Brightcove.MediaFramework.Brightcove.Entities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.MediaFramework.Entities;
using VideoSearchResult = Brightcove.MediaFramework.Brightcove.Indexing.Entities.VideoSearchResult;

namespace Brightcove.MediaFramework.Brightcove.Synchronize.Fallback
{
  public class VideoFallback : AssetFallback<VideoSearchResult>
  {
    protected override Item GetItem(object entity, Item accountItem)
    {
      Video video = (Video) entity;
      return accountItem.Axes.SelectSingleItem(string.Format("./Media Content//*[@@templateid='{0}' and @id='{1}']", (object) TemplateIDs.Video, (object) video.Id));
    }

    protected override MediaServiceSearchResult GetSearchResult(Item item)
    {
      VideoSearchResult videoSearchResult = (VideoSearchResult) base.GetSearchResult(item);
      videoSearchResult.CreationDate = item[FieldIDs.Video.CreationDate];
      videoSearchResult.LongDescription = item[FieldIDs.Video.LongDescription];
      videoSearchResult.PublishedDate = item[FieldIDs.Video.PublishedDate];
      videoSearchResult.LastModifiedDate = item[FieldIDs.Video.LastModifiedDate];
      videoSearchResult.Economics = item[FieldIDs.Video.Economics];
      videoSearchResult.LinkURL = item[FieldIDs.Video.LinkUrl];
      videoSearchResult.LinkText = item[FieldIDs.Video.LinkText];
      videoSearchResult.Tags = ID.ParseArray(item[FieldIDs.Video.Tags]);
      videoSearchResult.VideoStillURL = item[FieldIDs.Video.VideoStillUrl];
      videoSearchResult.CustomFields = item[FieldIDs.Video.CustomFields];
      return (MediaServiceSearchResult) videoSearchResult;
    }
  }
}
