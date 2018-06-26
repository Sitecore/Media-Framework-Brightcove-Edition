namespace Sitecore.MediaFramework.Pipelines.MediaExport.MediaItemExport
{
  public abstract class MediaItemExportProcessorBase
  {
    /// <summary>
    /// Ingests items
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public abstract void Process(MediaItemExportArgs args);
  }
}