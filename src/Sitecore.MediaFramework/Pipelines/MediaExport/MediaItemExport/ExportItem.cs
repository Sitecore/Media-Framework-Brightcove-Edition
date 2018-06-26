namespace Sitecore.MediaFramework.Pipelines.MediaExport.MediaItemExport
{
  using System;

  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Diagnostics;
  using Sitecore.MediaFramework.Export;

  public class ExportItem : MediaItemExportProcessorBase
  {
    /// <summary>
    /// Ingests items
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public override void Process(MediaItemExportArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.Operation, "args.Operation");

      try
      {
        IExportExecuter executer = MediaFrameworkContext.GetExportExecuter(args.Operation.Item);
        if (executer != null)
        {
          executer.Export(args.Operation);
        }
        else
        {
          args.AbortPipeline();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error("Sync failed.", this, ex);
        args.AbortPipeline();
      }
    }
  }
}