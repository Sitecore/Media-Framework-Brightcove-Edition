// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessQueue.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   Defines the CallItemExport type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Pipelines.MediaExport
{
  using System;

  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Diagnostics;
  using Sitecore.MediaFramework.Export;
  using Sitecore.MediaFramework.Pipelines.MediaExport.MediaItemExport;  
  using Sitecore.Pipelines;

  public class ProcessQueue
  {
    /// <summary>
    /// Call Item Import
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public void Process(PipelineArgs args)
    { 
      try
      {
        var operations = ExportQueueManager.GetAll();
        foreach (var operation in operations)
        {
          try
          {
            MediaItemExportPipeline.Run(new MediaItemExportArgs { Operation = operation });
          }
          catch (Exception ex)
          {
            LogHelper.Error("Error while exporting. Item Id:" + operation.Item.ID, this, ex);
          }
          finally
          {
            ExportQueueManager.Remove(operation);
          }         
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error("Export data failed.", this, ex);
        args.AbortPipeline();
      }
    }
  }
}