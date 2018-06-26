namespace Sitecore.MediaFramework.Pipelines.MediaSyncImport.MediaSyncItemImport
{
  using Sitecore.Diagnostics;
  using Sitecore.Pipelines;

  public class MediaSyncItemImportPipeline           
  {
    /// <summary>
    /// The run.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public static void Run(MediaSyncItemImportArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      CorePipeline.Run("mediaFramework.mediaSyncItemImport", args);
    }
  }
}