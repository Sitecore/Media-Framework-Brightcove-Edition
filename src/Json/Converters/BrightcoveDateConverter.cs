namespace Sitecore.MediaFramework.Brightcove.Json.Converters
{
  using System;
  using System.Globalization;

  using Newtonsoft.Json;

  /// <summary>
  /// Brightcove Date Converter
  /// </summary>
  public class BrightcoveDateConverter : JsonConverter
  {
    protected static readonly DateTime EpochDatetime = new DateTime(1970, 1, 1);

    /// <summary>
    /// To Unix Time
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected virtual string ToUnixTime(DateTime value)
    {
      var span = (value - EpochDatetime.ToLocalTime());
      return span.TotalSeconds.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Date Time
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected virtual DateTime DateFromUnix(string value)
    {
      var millisecs = double.Parse(value);
      var secs = millisecs / 1000;
      return EpochDatetime.AddSeconds(secs);
    }

    /// <summary>
    /// Write Json
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="serializer"></param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      DateTime? date = value as DateTime?;
      if (date != null)
      {
        var bcDate = ToUnixTime(date.GetValueOrDefault());
        writer.WriteValue(bcDate);
      }
      else
      {
        writer.WriteNull();
      }
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
      var value = reader.Value;
      if (value == null)
      {
        return null;
      }
      var convertedValue = DateTime.MinValue;
      if (typeof(DateTime?) == objectType)
      {
        convertedValue = DateFromUnix(value.ToString());
      }
      return convertedValue;
    }

    /// <summary>
    /// Can Convert
    /// </summary>
    /// <param name="objectType"></param>
    /// <returns></returns>
    public override bool CanConvert(Type objectType)
    {
      var value = typeof(DateTime?).IsAssignableFrom(objectType);
      return value;
    }
  }
}