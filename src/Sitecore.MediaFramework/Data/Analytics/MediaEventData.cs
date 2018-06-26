namespace Sitecore.MediaFramework.Data.Analytics
{
  using System;

  using Sitecore.Analytics.Model;

  [Serializable]
  public class MediaEventData
  {
    public string MediaId { get; set; }
    
    public string MediaName { get; set; }

    public int MediaLength { get; set; }
    
    public string EventParameter { get; set; }

    public static MediaEventData Parse(XConnect.Event pageEvent)
    {
      if (pageEvent == null || string.IsNullOrEmpty(pageEvent.Data))
      {
        return null;
      }

      string[] data = pageEvent.Data.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
      if (data.Length < 2)
      {
        return null;
      }

      int length;
      int.TryParse(data[1], out length);

      return new MediaEventData
      {
        MediaId = pageEvent.DataKey,
        MediaName = data[0],
        MediaLength = length,
        EventParameter = data.Length >= 3 ? data[2] : string.Empty
      };
    }
  }
}