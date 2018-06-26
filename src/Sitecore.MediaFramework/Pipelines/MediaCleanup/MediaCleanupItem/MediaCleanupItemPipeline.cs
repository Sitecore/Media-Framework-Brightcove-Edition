namespace Sitecore.MediaFramework.Pipelines.MediaCleanup.MediaCleanupItem
{
  using Sitecore.Diagnostics;
  using Sitecore.Pipelines;

  /// <summary>
  /// The media cleanup Item pipeline.
  /// </summary>
  public class MediaCleanupItemPipeline
  {
    /// <summary>
    /// The run.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public static void Run(MediaCleanupItemArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      CorePipeline.Run("mediaFramework.mediaCleanupItem", args);
    }
  }
}
