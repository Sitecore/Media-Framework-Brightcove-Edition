namespace Sitecore.MediaFramework.Pipelines.MediaExport
{                             
  using Sitecore.Pipelines;

  public class MediaExportPipeline
  {
    /// <summary>
    /// The run.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public static void Run(PipelineArgs args)
    {                                        
      CorePipeline.Run("mediaFramework.mediaExport", args);
    }
  }
}