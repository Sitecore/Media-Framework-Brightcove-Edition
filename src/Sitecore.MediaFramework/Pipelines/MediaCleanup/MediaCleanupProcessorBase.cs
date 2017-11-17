namespace Sitecore.MediaFramework.Pipelines.MediaCleanup
{
  public abstract class MediaCleanupProcessorBase
  {
    /// <summary>
    /// Ingests items
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public abstract void Process(MediaCleanupArgs args);
  }
}