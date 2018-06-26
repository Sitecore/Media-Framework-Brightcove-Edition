namespace Sitecore.MediaFramework.Data.Analytics
{
  using Sitecore.Analytics.Aggregation.Data.Model;
  using Sitecore.Analytics.Core;

  public class MediaFrameworkMediaKey : DictionaryKey
  {
    private readonly Hash128 mediaId;

    public Hash128 MediaId { get { return this.mediaId; } }

    public MediaFrameworkMediaKey(Hash128 mediaId)
    {
      this.mediaId = mediaId;
    }

    public MediaFrameworkMediaKey(string mediaId)
      : this(Hash128.Compute(mediaId))
    {
    }
  }
}