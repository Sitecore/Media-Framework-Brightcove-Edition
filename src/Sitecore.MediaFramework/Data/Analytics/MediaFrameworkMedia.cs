namespace Sitecore.MediaFramework.Data.Analytics
{
  using Sitecore.Analytics.Aggregation.Data.Model;
  using Sitecore.Analytics.Core;

  public class MediaFrameworkMedia : Dimension<MediaFrameworkMediaKey, MediaFrameworkMediaValue>
  {
    public Hash128 AddValue(MediaEventData eventData)
    {
      return this.AddValue(new MediaFrameworkMediaKey(eventData.MediaId), new MediaFrameworkMediaValue
        {
          Name = eventData.MediaName,
          Length = eventData.MediaLength
        });
    }

    public Hash128 AddValue(string mediaId, MediaFrameworkMediaValue value)
    {
      return this.AddValue(new MediaFrameworkMediaKey(mediaId), value);
    }

    public Hash128 AddValue(MediaFrameworkMediaKey key, MediaFrameworkMediaValue value)
    {
      base.Add(key, value);

      return key.MediaId;
    }
  }
}