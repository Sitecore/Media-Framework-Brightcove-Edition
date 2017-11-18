
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Brightcove.MediaFramework.Brightcove.Entities;
using Newtonsoft.Json;


namespace Brightcove.MediaFramework.Brightcove.Json.Converters
{
    public class BrightcovePlaylistSearchFieldConverter : JsonConverter
    {
        protected static readonly DateTime EpochDatetime = new DateTime(1970, 1, 1);

        protected virtual string ToSearchString(PlayListSearch playListSearch)
        {
            var result = string.Format(CultureInfo.CurrentCulture, "tags:{0}", string.Join(",", playListSearch.FilterTags.Select(i => string.Format(CultureInfo.CurrentCulture, "\"{0}\"", i))));
            if (playListSearch.TagInclusion == TagInclusion.AND)
            {
                result = result.Insert(0, "+");
            }

            return result;
        }

        protected virtual PlayListSearch ToList(string value)
        {
            PlayListSearch result = null;
            if (!string.IsNullOrEmpty(value))
            {
                result = new PlayListSearch { TagInclusion = TagInclusion.OR };
                if (value.Substring(0, 1).Equals("+", StringComparison.OrdinalIgnoreCase))
                {
                    result.TagInclusion = TagInclusion.AND;
                    value = value.Substring(1);
                }

                value = value.Replace("tags:", "");
                if (!string.IsNullOrEmpty(value))
                {
                    result.FilterTags= value.Split(new string[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(i => i.Trim('\"')).ToList();
                }
            }

            return result;
        }

        /// <summary>
        /// Write Json
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var playListSearch = value as PlayListSearch;
            if (playListSearch != null && playListSearch.FilterTags != null && playListSearch.FilterTags.Count > 0)
            {
                string str = ToSearchString(playListSearch);
                writer.WriteValue(str);
            }
            else
                writer.WriteNull();
        }

        /// <summary>
        /// Read Json
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object obj = reader.Value;
            if (obj == null)
                return (object)null;
            var result = this.ToList(obj.ToString());
            return (object)result;
        }

        /// <summary>
        /// Can Convert
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
