namespace Sitecore.MediaFramework.Brightcove.Synchronize.EntityCreators
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Web;

  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Brightcove.Entities;
  using Sitecore.MediaFramework.Synchronize;

  using Integration.Common.Utils;

  /// <summary>
  /// Video Entity Creator
  /// </summary>
  public class VideoEntityCreator : IMediaServiceEntityCreator
  {
    /// <summary>
    ///  Entity From Item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public virtual object CreateEntity(Item item)
    {
      Economics economics;

      DateTime creationDate;
      DateTime lastModifiedDate;
      DateTime publishedDate;

      Enum.TryParse(item[FieldIDs.Video.Economics], true, out economics);

      DateTime.TryParse(item[FieldIDs.Video.CreationDate], out creationDate);
      DateTime.TryParse(item[FieldIDs.Video.LastModifiedDate], out lastModifiedDate);
      DateTime.TryParse(item[FieldIDs.Video.PublishedDate], out publishedDate);

      return new Video
      {
        Id = item[FieldIDs.MediaElement.Id],
        Economics = economics,
        ReferenceId = item[FieldIDs.MediaElement.ReferenceId],
        Name = item[FieldIDs.MediaElement.Name],
        Length =  MainUtil.GetLong(item[FieldIDs.Video.Length],0),
        CreationDate = creationDate,
        LastModifiedDate = lastModifiedDate,
        PublishedDate = publishedDate,
        LinkText = item[FieldIDs.Video.LinkText],
        LinkURL = item[FieldIDs.Video.LinkUrl],
        LongDescription = item[FieldIDs.Video.LongDescription],
        PlaysTotal = MainUtil.GetInt(item[FieldIDs.Video.PlaysTotal],0),
        PlaysTrailingWeek = MainUtil.GetInt(item[FieldIDs.Video.PlaysTrailingWeek],0),
        ShortDescription = item[FieldIDs.MediaElement.ShortDescription],
        ThumbnailUrl = item[FieldIDs.MediaElement.ThumbnailUrl],
        VideoStillURL = item[FieldIDs.Video.VideoStillUrl],
        Tags = this.GetValues(item.Fields[FieldIDs.Video.Tags], FieldIDs.Tag.Name),
        CustomFields = this.GetCustomFields(item.Fields[FieldIDs.Video.CustomFields])
      };
    }

    /// <summary>
    /// Get Values
    /// </summary>
    /// <param name="field"></param>
    /// <param name="selectFieldId"></param>
    /// <returns></returns>
    protected virtual List<string> GetValues(Field field, ID selectFieldId)
    {
      return ((MultilistField)field).GetItems().Select(i => i[selectFieldId]).ToList();
    }

    protected virtual Dictionary<string, string> GetCustomFields(Field field)
    {
      var tmp = StringUtil.GetDictionary(field.Value, '=', '&');

      var result = new Dictionary<string, string>(tmp.Count);

      foreach (var pair in tmp)
      {
        result.Add(pair.Key, HttpUtility.UrlDecode(pair.Value));
      }

      return result;
    }
  }
}