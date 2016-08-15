namespace Sitecore.MediaFramework.Brightcove.Synchronize.EntityCreators
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Brightcove.Entities;
  using Sitecore.MediaFramework.Brightcove.Indexing.Entities;
  using Sitecore.MediaFramework.Entities;
  using Sitecore.MediaFramework.Synchronize;

  /// <summary>
  /// PlayList Entity Creator
  /// </summary>
  public class PlayListEntityCreator : IMediaServiceEntityCreator
  {
    /// <summary>
    /// Entity From Item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public virtual object CreateEntity(Item item)
    {
      PlaylistType playlistType;
      Enum.TryParse(item[FieldIDs.PlayerList.PlaylistType], true, out playlistType);

      TagInclusion tagInclusion;
      Enum.TryParse(item[FieldIDs.PlayerList.TagInclusion], true, out tagInclusion);

      return new PlayList
      {
        Id = item[FieldIDs.MediaElement.Id],
        ReferenceId = item[FieldIDs.MediaElement.ReferenceId],
        ShortDescription = item[FieldIDs.MediaElement.ShortDescription],
        Name = item[FieldIDs.MediaElement.Name],
        PlaylistType = playlistType.ToString(),
        ThumbnailUrl = item[FieldIDs.MediaElement.ThumbnailUrl],
        VideoIds = this.GetValues(item.Fields[FieldIDs.PlayerList.VideoIds],FieldIDs.MediaElement.Id),
        TagInclusion = tagInclusion.ToString(),
        FilterTags = this.GetValues(item.Fields[FieldIDs.PlayerList.FilterTags], FieldIDs.Tag.Name)
      };
    }

    protected virtual List<string> GetValues(Field field, ID selectFieldId)
    {
      return ((MultilistField)field).GetItems().Select(i => i[selectFieldId]).ToList();
    }
  }
}