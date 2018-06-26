namespace Sitecore.MediaFramework.Schedulers
{
  using System;

  using Sitecore.MediaFramework.Pipelines.MediaExport;
  using Sitecore.Pipelines;

  [Obsolete]
  public class ExportScheduler
  {
    protected virtual void Execute()
    {
      MediaExportPipeline.Run(new PipelineArgs());
    }
  }
}