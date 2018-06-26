namespace Sitecore.MediaFramework.Pipelines.MediaExport.MediaItemExport
{
  using Sitecore.Diagnostics;
  using Sitecore.Pipelines;

  public class MediaItemExportPipeline           
  {
    /// <summary>
    /// The run.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public static void Run(MediaItemExportArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      CorePipeline.Run("mediaFramework.mediaItemExport", args);
    }
  }
}