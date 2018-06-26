// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExportExecuter.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   The export executer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Export
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Diagnostics;
  using Sitecore.MediaFramework.Pipelines.MediaSyncImport.MediaSyncItemImport;
  using Sitecore.MediaFramework.Synchronize;
  using Sitecore.Pipelines.Save;

  /// <summary>
  /// The export executer.
  /// </summary>
  public abstract class ExportExecuterBase : IExportExecuter
  {
    [Obsolete("Use MediaFrameworkContext.GetItemSynchronizer method")]
    public IItemSynchronizer Synchronizer { get; set; }

    //protected List<ID> FieldsToUpdate { get; private set; }
    protected List<ID> FieldsToInspect { get; private set; }

    protected ExportExecuterBase()
    {
      this.FieldsToInspect = new List<ID>();
    }

    public void AddFieldToInspect(string fieldId)
    {
      if (ID.IsID(fieldId))
      {
        this.FieldsToInspect.Add(new ID(fieldId));
      }
    }

    /// <summary>
    /// Process export operation.
    /// </summary>
    /// <param name="operation">
    /// The operation.
    /// </param>
    public virtual void Export(ExportOperation operation)
    {
      LogHelper.Debug("Export operation:" + operation, this);

      switch (operation.Type)
      {
        case ExportOperationType.Update:
          var entity = this.Update(operation);
          this.UpdateOnSitecore(operation, entity);
          break;

        case ExportOperationType.Create:
          entity = this.Create(operation);
          this.UpdateOnSitecore(operation, entity);
          break;

        case ExportOperationType.Delete:
          this.Delete(operation);
          break;

        case ExportOperationType.Move:
          entity = this.Move(operation);
          this.UpdateOnSitecore(operation, entity);
          break;
      }
    }

    public abstract bool IsNew(Item item);

    /// <summary>
    /// Creates a media element.
    /// </summary>
    protected abstract object Create(ExportOperation operation);

    /// <summary>
    /// Deletes a media element.
    /// </summary>
    protected abstract void Delete(ExportOperation operation);

    /// <summary>
    /// Updates a media element.
    /// </summary>
    protected abstract object Update(ExportOperation operation);

    /// <summary>
    /// Move a media element.
    /// </summary>
    protected virtual object Move(ExportOperation operation)
    {
      return null;
    }

    /// <summary>
    /// Updates media item on the Sitecore after update on service side.
    /// </summary>
    protected virtual void UpdateOnSitecore(ExportOperation operation, object entity)
    {
      IItemSynchronizer sync = MediaFrameworkContext.GetItemSynchronizer(entity);
      if (sync != null)
      {
        var args = new MediaSyncItemImportArgs
        {
          Entity = entity,
          Item = operation.Item,
          AccountItem = operation.AccountItem,
          Synchronizer = sync,
          SyncAllowActivity = SyncAllowActivity.UpdateItem | SyncAllowActivity.SyncReferences
        };
        MediaSyncItemImportPipeline.Run(args);

        //sync.UpdateItem(entity, operation.AccountItem, operation.Item);
        //sync.SyncReferences(entity, operation.AccountItem, operation.Item);
      }
    }

    public virtual bool NeedToUpdate(SaveArgs.SaveItem item)
    {
      return item.Fields.Any(field => this.FieldsToInspect.Contains(field.ID) && field.Value != field.OriginalValue);
    }
  }
}