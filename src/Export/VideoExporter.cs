namespace Sitecore.MediaFramework.Brightcove.Export
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Brightcove.Entities;
  using Sitecore.MediaFramework.Brightcove.Security;
  using Sitecore.MediaFramework.Export;
  using Sitecore.RestSharp;

  /// <summary>
  /// Video Exporter
  /// </summary>
  public abstract class VideoExporter : ExportExecuterBase
  {
    /// <summary>
    /// Create
    /// </summary>
    /// <param name="operation"></param>
    /// <returns></returns>
    protected override object Create(ExportOperation operation)
    {
      return null;
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="operation"></param>
    /// <returns></returns>
    protected override object Update(ExportOperation operation)
    {
      var synchronizer = MediaFrameworkContext.GetItemSynchronizer(operation.Item);
      if (synchronizer == null)
      {
        return null;
      }

      var video = (Video)synchronizer.CreateEntity(operation.Item);

      video.ThumbnailUrl = null;
      video.CreationDate = null;
      video.VideoStillURL = null;
      video.PublishedDate = null;
      video.PlaysTrailingWeek = null;
      video.PlaysTotal = null;
      video.LastModifiedDate = null;
      video.Length = null;

      if (video.CustomFields != null && video.CustomFields.Count == 0)
      {
        video.CustomFields = null;
      }

      var authenticator = new BrightcoveAthenticator(operation.AccountItem);

      var context = new RestContext(Constants.SitecoreRestSharpService, authenticator);

      var updateData = new PostData("update_video", authenticator, "video", video);

      var data = context.Update<PostData, ResultData<Video>>("update_data", updateData).Data;

      if (data != null && data.Result != null)
      {
        if (data.Result.CustomFields == null)
        {
          data.Result.CustomFields = video.CustomFields;
        }

        return data.Result;
      }

      return null;
    }

    /// <summary>
    /// Is New
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public override bool IsNew(Item item)
    {
      return item[FieldIDs.MediaElement.Id].Length == 0;
    }
  }
}