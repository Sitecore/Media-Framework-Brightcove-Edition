
namespace Sitecore.MediaFramework.Pipelines.MediaSyncImport
{
  public abstract class MediaSyncImportProcessorBase
  {
    /// <summary>
    /// Ingests items
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public abstract void Process(MediaSyncImportArgs args);
  }
}