using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AgencyOasis.MediaFramework.Brightcove.Entities;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.MediaFramework.Brightcove;
using Sitecore.MediaFramework.Brightcove.Entities;
using Sitecore.MediaFramework.Synchronize;
using PlayList = AgencyOasis.MediaFramework.Brightcove.Entities.PlayList;
using PlaylistType = AgencyOasis.MediaFramework.Brightcove.Entities.PlaylistType;

namespace AgencyOasis.MediaFramework.Brightcove.Synchronize.EntityCreators
{
  public class PlayListEntityCreator : IMediaServiceEntityCreator
  {
    public virtual object CreateEntity(Item item)
    {
      PlaylistType result1;
      Enum.TryParse<PlaylistType>(item[Sitecore.MediaFramework.Brightcove.FieldIDs.PlayerList.PlaylistType], true, out result1);
      TagInclusion result2 = TagInclusion.OR;
      Enum.TryParse<TagInclusion>(item[Sitecore.MediaFramework.Brightcove.FieldIDs.PlayerList.TagInclusion], true, out result2);
      PlayList playList = new PlayList();
      playList.Id = item[Sitecore.MediaFramework.Brightcove.FieldIDs.MediaElement.Id];
      playList.ReferenceId = item[Sitecore.MediaFramework.Brightcove.FieldIDs.MediaElement.ReferenceId];
      playList.ShortDescription = item[Sitecore.MediaFramework.Brightcove.FieldIDs.MediaElement.ShortDescription];
      playList.Name = item[Sitecore.MediaFramework.Brightcove.FieldIDs.MediaElement.Name];
      playList.PlaylistType = result1.ToString();
      playList.VideoIds = this.GetValues(item.Fields[Sitecore.MediaFramework.Brightcove.FieldIDs.PlayerList.VideoIds], Sitecore.MediaFramework.Brightcove.FieldIDs.MediaElement.Id);
        playList.PlayListSearch = new PlayListSearch();
        playList.PlayListSearch.TagInclusion = result2;
      playList.PlayListSearch.FilterTags = this.GetValues(item.Fields[Sitecore.MediaFramework.Brightcove.FieldIDs.PlayerList.FilterTags], Sitecore.MediaFramework.Brightcove.FieldIDs.Tag.Name);
      return (object) playList;
    }

    protected virtual List<string> GetValues(Field field, ID selectFieldId)
    {
      return Enumerable.ToList<string>(Enumerable.Select<Item, string>((IEnumerable<Item>) ((MultilistField) field).GetItems(), (Func<Item, string>) (i => i[selectFieldId])));
    }
  }
}
