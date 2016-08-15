
namespace Sitecore.MediaFramework.Brightcove.Synchronize
{
  using System;
  using System.Web;

  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Brightcove.Entities;
  using Sitecore.MediaFramework.Brightcove.Indexing.Entities;
  using Sitecore.MediaFramework.Entities;

  using Integration.Common.Utils;

  public class VideoSynchronizer : AssetSynchronizer
  {
    public override Item SyncItem(object entity, Item accountItem)
    {
      var video = (Video)entity;

      if (video.ItemState == Entities.ItemState.INACTIVE)
      {
        return null;
      }

      return base.SyncItem(entity, accountItem);
    }

    public override Item UpdateItem(object entity, Item accountItem, Item item)
    {
      var video = (Video)entity;
      using (new EditContext(item))
      {
        item.Name = ItemUtil.ProposeValidItemName(video.Name);

        item[FieldIDs.MediaElement.Id] = video.Id;
        item[FieldIDs.MediaElement.Name] = video.Name;
        item[FieldIDs.MediaElement.ReferenceId] = video.ReferenceId;
        item[FieldIDs.MediaElement.ThumbnailUrl] = video.ThumbnailUrl;
        item[FieldIDs.MediaElement.ShortDescription] = video.ShortDescription;
        item[FieldIDs.Video.CreationDate] = video.CreationDate.ToString();
        item[FieldIDs.Video.LongDescription] = video.LongDescription;
        item[FieldIDs.Video.PublishedDate] = video.PublishedDate.ToString();
        item[FieldIDs.Video.LastModifiedDate] = video.LastModifiedDate.ToString();
        item[FieldIDs.Video.Economics] = video.Economics.ToString();
        item[FieldIDs.Video.LinkUrl] = video.LinkURL;
        item[FieldIDs.Video.LinkText] = video.LinkText;
        item[FieldIDs.Video.VideoStillUrl] = video.VideoStillURL;
        item[FieldIDs.Video.Length] = video.Length.ToString();
        item[FieldIDs.Video.PlaysTotal] = video.PlaysTotal.ToString();
        item[FieldIDs.Video.PlaysTrailingWeek] = video.PlaysTrailingWeek.ToString();

        item[FieldIDs.Video.CustomFields] = this.GetCustomFields(video);
      }

      return item;
    }

    public override bool NeedUpdate(object entity, Item accountItem, MediaServiceSearchResult searchResult)
    {
      var video = (Video)entity;
      var videoIndex = (VideoSearchResult)searchResult;

      return !StringUtil.EqualsIgnoreNullEmpty(video.LastModifiedDate.ToString(), videoIndex.LastModifiedDate)
             || !StringUtil.EqualsIgnoreNullEmpty(video.PlaysTotal.ToString(), videoIndex.PlaysTotal, StringComparison.OrdinalIgnoreCase)
             || !StringUtil.EqualsIgnoreNullEmpty(video.PlaysTrailingWeek.ToString(), videoIndex.PlaysTrailingWeek, StringComparison.OrdinalIgnoreCase)
             || !StringUtil.EqualsIgnoreNullEmpty(video.ThumbnailUrl, videoIndex.ThumbnailUrl)
             || !StringUtil.EqualsIgnoreNullEmpty(video.VideoStillURL, videoIndex.VideoStillURL);
    }

    public override MediaServiceSearchResult GetSearchResult(object entity, Item accountItem)
    {
      var video = (Video)entity;

      return base.GetSearchResult<VideoSearchResult>(Constants.IndexName, accountItem, i => (i.TemplateId == TemplateIDs.Video) && i.Id == video.Id);
    }

    public override MediaServiceEntityData GetMediaData(object entity)
    {
      var mediaData = base.GetMediaData(entity);

      mediaData.TemplateId = TemplateIDs.Video;

      return mediaData;
    }

    protected virtual string GetCustomFields(Video video)
    {
      if (video.CustomFields == null)
      {
        return null;
      }

      return HttpUtility.UrlPathEncode(StringUtil.DictionaryToString(video.CustomFields, '=', '&'));
    }
  }
}