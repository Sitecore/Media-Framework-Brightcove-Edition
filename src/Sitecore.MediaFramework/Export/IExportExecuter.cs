// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExportExecuter.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   Defines the IExportExecuter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Export
{
  using Sitecore.Data.Items;
  using Sitecore.Pipelines.Save;

  /// <summary>
  /// The export executer interface.
  /// </summary>
  public interface IExportExecuter
  {
    /// <summary>
    /// The is new.
    /// </summary>
    /// <param name="item">
    /// The item.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    bool IsNew(Item item); 

    /// <summary>
    /// Processes export operation.
    /// </summary>
    /// <param name="operation">
    /// The operation.
    /// </param>
    void Export(ExportOperation operation);

    /// <summary>
    /// The need to update.
    /// </summary>
    /// <param name="item">
    /// The item.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    bool NeedToUpdate(SaveArgs.SaveItem item);
  }
}