namespace Sitecore.MediaFramework.Pipelines.MediaSyncImport
{
  using Sitecore.Diagnostics;
  using Sitecore.Pipelines;

  public class MediaSyncImportPipeline
  {
    /// <summary>
    /// The run.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public static void Run(MediaSyncImportArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      CorePipeline.Run("mediaFramework.mediaSyncImport", args);
    }
  }
}