// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetData.cs"  company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   The get data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Pipelines.MediaSyncImport
{
  using System;

  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Diagnostics;
  using Sitecore.MediaFramework.Import;

  /// <summary>
  /// The get data.
  /// </summary>
  public class GetData : MediaSyncImportProcessorBase
  {
    /// <summary>
    /// Ingests items
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public override void Process(MediaSyncImportArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.AccountItem, "args.AccountItem");
      Assert.ArgumentNotNullOrEmpty(args.ImportName, "args.ImportName");

      try
      {
        if (args.ResultData == null)
        {
          args.ResultData = ImportManager.Import(args.ImportName, args.AccountItem);

          if (args.ResultData == null)
          {
            LogHelper.Warn(string.Format("ResultData is null. ImportName:{0}; AccountId:{1}", args.ImportName, args.AccountItem.ID), this);
            args.AbortPipeline();
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error("Import data failed.", this, ex);
        args.AbortPipeline();
      }
    }
  }
}