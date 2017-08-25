using AgencyOasis.MediaFramework.Brightcove.Entities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.MediaFramework.Brightcove;
using Sitecore.MediaFramework.Brightcove.Synchronize.Fallback;
using Sitecore.MediaFramework.Entities;
using VideoSearchResult = AgencyOasis.MediaFramework.Brightcove.Indexing.Entities.VideoSearchResult;

namespace AgencyOasis.MediaFramework.Brightcove.Synchronize.Fallback
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
      videoSearchResult.CreationDate = item[Sitecore.MediaFramework.Brightcove.FieldIDs.Video.CreationDate];
      videoSearchResult.LongDescription = item[Sitecore.MediaFramework.Brightcove.FieldIDs.Video.LongDescription];
      videoSearchResult.PublishedDate = item[Sitecore.MediaFramework.Brightcove.FieldIDs.Video.PublishedDate];
      videoSearchResult.LastModifiedDate = item[Sitecore.MediaFramework.Brightcove.FieldIDs.Video.LastModifiedDate];
      videoSearchResult.Economics = item[Sitecore.MediaFramework.Brightcove.FieldIDs.Video.Economics];
      videoSearchResult.LinkURL = item[Sitecore.MediaFramework.Brightcove.FieldIDs.Video.LinkUrl];
      videoSearchResult.LinkText = item[Sitecore.MediaFramework.Brightcove.FieldIDs.Video.LinkText];
      videoSearchResult.Tags = ID.ParseArray(item[Sitecore.MediaFramework.Brightcove.FieldIDs.Video.Tags]);
      videoSearchResult.VideoStillURL = item[Sitecore.MediaFramework.Brightcove.FieldIDs.Video.VideoStillUrl];
      videoSearchResult.CustomFields = item[Sitecore.MediaFramework.Brightcove.FieldIDs.Video.CustomFields];
      return (MediaServiceSearchResult) videoSearchResult;
    }
  }
}
