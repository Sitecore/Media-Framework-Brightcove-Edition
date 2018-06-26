using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Brightcove.MediaFramework.Brightcove.Entities;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;


using Sitecore.MediaFramework.Synchronize;
using PlayList = Brightcove.MediaFramework.Brightcove.Entities.PlayList;
using PlaylistType = Brightcove.MediaFramework.Brightcove.Entities.PlaylistType;

namespace Brightcove.MediaFramework.Brightcove.Synchronize.EntityCreators
{
  public class PlayListEntityCreator : IMediaServiceEntityCreator
  {
    public virtual object CreateEntity(Item item)
    {
      PlaylistType result1;
      Enum.TryParse<PlaylistType>(item[FieldIDs.PlayerList.PlaylistType], true, out result1);
      TagInclusion result2 = TagInclusion.OR;
      Enum.TryParse<TagInclusion>(item[FieldIDs.PlayerList.TagInclusion], true, out result2);
      PlayList playList = new PlayList();
      playList.Id = item[FieldIDs.MediaElement.Id];
      playList.ReferenceId = item[FieldIDs.MediaElement.ReferenceId];
      playList.ShortDescription = item[FieldIDs.MediaElement.ShortDescription];
      playList.Name = item[FieldIDs.MediaElement.Name];
      playList.PlaylistType = result1.ToString();
      playList.VideoIds = this.GetValues(item.Fields[FieldIDs.PlayerList.VideoIds], FieldIDs.MediaElement.Id);
        playList.PlayListSearch = new PlayListSearch();
        playList.PlayListSearch.TagInclusion = result2;
      playList.PlayListSearch.FilterTags = this.GetValues(item.Fields[FieldIDs.PlayerList.FilterTags], FieldIDs.Tag.Name);
      return (object) playList;
    }

    protected virtual List<string> GetValues(Field field, ID selectFieldId)
    {
      return Enumerable.ToList<string>(Enumerable.Select<Item, string>((IEnumerable<Item>) ((MultilistField) field).GetItems(), (Func<Item, string>) (i => i[selectFieldId])));
    }
  }
}
