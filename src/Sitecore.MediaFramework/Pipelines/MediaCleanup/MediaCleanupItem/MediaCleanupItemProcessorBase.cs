namespace Sitecore.MediaFramework.Pipelines.MediaCleanup.MediaCleanupItem
{
  public abstract class MediaCleanupItemProcessorBase
  {
    /// <summary>
    /// Ingests items
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public abstract void Process(MediaCleanupItemArgs args);
  }
}