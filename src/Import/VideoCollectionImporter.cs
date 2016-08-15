namespace Sitecore.MediaFramework.Brightcove.Import
{
  using Sitecore.MediaFramework.Brightcove.Entities;

  /// <summary>
  /// Video Collection Importer
  /// </summary>
  public class VideoCollectionImporter : EntityCollectionImporter<Video>
  {
    /// <summary>
    /// Request Name
    /// </summary>
    protected override string RequestName
    {
      get
      {
        return "read_videos";
      }
    }
  }
}