using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Brightcove.MediaFramework.Brightcove.Entities;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.MediaFramework.Synchronize;
using Video = Brightcove.MediaFramework.Brightcove.Entities.Video;

namespace Brightcove.MediaFramework.Brightcove.Synchronize.EntityCreators
{
    public class VideoEntityCreator : IMediaServiceEntityCreator
    {
        public virtual object CreateEntity(Item item)
        {
            Economics result1;
            Enum.TryParse<Economics>(item[FieldIDs.Video.Economics], true, out result1);
            DateTime result2;
            DateTime.TryParse(item[FieldIDs.Video.CreationDate], out result2);
            DateTime result3;
            DateTime.TryParse(item[FieldIDs.Video.LastModifiedDate], out result3);
            DateTime result4;
            DateTime.TryParse(item[FieldIDs.Video.PublishedDate], out result4);
            var video = new Video();
            video.Id = item[FieldIDs.MediaElement.Id];
            video.Economics = new Economics?(result1);
            video.ReferenceId = item[FieldIDs.MediaElement.ReferenceId];
            video.Name = item[FieldIDs.MediaElement.Name];
            video.CreationDate = new DateTime?(result2);
            video.LastModifiedDate = new DateTime?(result3);
            video.PublishedDate = new DateTime?(result4);
            video.Link = new VideoLink();
            video.Link.Text = item[FieldIDs.Video.LinkText];
            video.Link.URL = item[FieldIDs.Video.LinkUrl];
            video.LongDescription = item[FieldIDs.Video.LongDescription];
            video.ShortDescription = item[FieldIDs.MediaElement.ShortDescription];
            video.Images = new ImageAssets();
            video.Images.Thumbnail = new ImageAsset();
            video.Images.Thumbnail.Sources = new List<AssetSource>();
            video.Images.Poster = new ImageAsset();
            video.Images.Poster.Sources = new List<AssetSource>();
            video.Images.Thumbnail.Src = item[FieldIDs.MediaElement.ThumbnailUrl];
            video.Images.Poster.Src = item[FieldIDs.Video.VideoStillUrl];
            video.Tags = this.GetValues(item.Fields[FieldIDs.Video.Tags], FieldIDs.Tag.Name);
            video.CustomFields = this.GetCustomFields(item.Fields[FieldIDs.Video.CustomFields]);
            return (object)video;
        }

        protected virtual List<string> GetValues(Field field, ID selectFieldId)
        {
            return Enumerable.ToList<string>(Enumerable.Select<Item, string>((IEnumerable<Item>)((MultilistField)field).GetItems(), (Func<Item, string>)(i => i[selectFieldId])));
        }

        protected virtual Dictionary<string, string> GetCustomFields(Field field)
        {
            Dictionary<string, string> dictionary1 = Sitecore.Integration.Common.Utils.StringUtil.GetDictionary(field.Value, '=', '&');
            Dictionary<string, string> dictionary2 = new Dictionary<string, string>(dictionary1.Count);
            foreach (KeyValuePair<string, string> keyValuePair in dictionary1)
                dictionary2.Add(keyValuePair.Key, HttpUtility.UrlDecode(keyValuePair.Value));
            return dictionary2;
        }
    }
}
