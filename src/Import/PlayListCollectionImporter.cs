namespace Sitecore.MediaFramework.Brightcove.Import
{
  using Sitecore.MediaFramework.Brightcove.Entities;

  /// <summary>
  /// PlayList Collection Importer
  /// </summary>
  public class PlayListCollectionImporter : EntityCollectionImporter<PlayList>
  {
    protected override string RequestName
    {
      get
      {
        return "read_playlists";
      }
    }
  }
}