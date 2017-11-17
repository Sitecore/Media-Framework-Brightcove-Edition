namespace Sitecore.MediaFramework.Pipelines.MediaCleanup
{
  using Sitecore.Diagnostics;
  using Sitecore.Pipelines;

  public class MediaCleanupPipeline
  {
    /// <summary>
    /// The run.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public static void Run(MediaCleanupArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      CorePipeline.Run("mediaFramework.mediaCleanup", args);
    }
  }
}
