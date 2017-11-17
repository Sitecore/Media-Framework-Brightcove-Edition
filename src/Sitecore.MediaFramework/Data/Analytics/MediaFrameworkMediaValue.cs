namespace Sitecore.MediaFramework.Data.Analytics
{
  using Sitecore.Analytics.Aggregation.Data.Model;

  public class MediaFrameworkMediaValue : DictionaryValue
  {
    public string Name { get; set; }
    public int Length { get; set; }
  }
}