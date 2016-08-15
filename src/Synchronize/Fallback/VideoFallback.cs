namespace Sitecore.MediaFramework.Brightcove.Synchronize.Fallback
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Brightcove.Entities;
  using Sitecore.MediaFramework.Brightcove.Indexing.Entities;
  using Sitecore.MediaFramework.Entities;

  /// <summary>
  /// Video Fallback
  /// </summary>
  public class VideoFallback : AssetFallback<VideoSearchResult>
  {
    /// <summary>
    /// Get Item
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="accountItem"></param>
    /// <returns></returns>
    protected override Item GetItem(object entity, Item accountItem)
    {
      var video = (Video)entity;
      return accountItem.Axes.SelectSingleItem(string.Format("./Media Content//*[@@templateid='{0}' and @id='{1}']", TemplateIDs.Video, video.Id));
    }

    /// <summary>
    /// Get Index
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    protected override MediaServiceSearchResult GetSearchResult(Item item)
    {
      var searchResult = (VideoSearchResult)base.GetSearchResult(item);

      searchResult.CreationDate = item[FieldIDs.Video.CreationDate];
      searchResult.LongDescription = item[FieldIDs.Video.LongDescription];
      searchResult.PublishedDate = item[FieldIDs.Video.PublishedDate];
      searchResult.LastModifiedDate = item[FieldIDs.Video.LastModifiedDate];
      searchResult.Economics = item[FieldIDs.Video.Economics];
      searchResult.LinkURL = item[FieldIDs.Video.LinkUrl];
      searchResult.LinkText = item[FieldIDs.Video.LinkText];
      searchResult.Tags = ID.ParseArray(item[FieldIDs.Video.Tags]);
      searchResult.VideoStillURL = item[FieldIDs.Video.VideoStillUrl];
      searchResult.Length = item[FieldIDs.Video.Length];
      searchResult.PlaysTotal = item[FieldIDs.Video.PlaysTotal];
      searchResult.PlaysTrailingWeek = item[FieldIDs.Video.PlaysTrailingWeek];
      searchResult.CustomFields = item[FieldIDs.Video.CustomFields];

      return searchResult;
    }
  }
}