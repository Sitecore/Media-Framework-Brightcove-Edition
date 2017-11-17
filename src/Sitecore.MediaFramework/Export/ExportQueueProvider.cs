namespace Sitecore.MediaFramework.Export
{
  using System;
  using System.Collections.Generic;
  using System.Configuration.Provider;
  using System.Threading;

  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Pipelines.MediaExport;
  using Sitecore.Pipelines;

  public abstract class ExportQueueProvider : ProviderBase
  {
    protected static bool WaitFoProcessing;

    public virtual void Add(ExportOperation operation)
    {
      if (!WaitFoProcessing)
      {
        ThreadPool.QueueUserWorkItem(this.ProcesQueue);
        WaitFoProcessing = true;
      }
    }

    public abstract void Remove(ExportOperation operation);

    public abstract bool IsExist(Func<ExportOperation, bool> predicate);

    public abstract bool IsExist(ExportOperationType type, Item accountItem, ID fieldId, string fieldValue);

    public virtual bool IsExist(Item accountItem, ID fieldId, string fieldValue)
    {
      return this.IsExist(ExportOperationType.Delete | ExportOperationType.Update | ExportOperationType.Move, accountItem, fieldId, fieldValue);
    }

    public abstract ExportOperation Get(Func<ExportOperation, bool> predicate);

    public abstract List<ExportOperation> GetList(Func<ExportOperation, bool> predicate);

    public abstract List<ExportOperation> GetAll();

    protected virtual void ProcesQueue(object state)
    {
      Thread.Sleep(MediaFrameworkContext.ExportTimeout * 1000);

      WaitFoProcessing = false;
      
      MediaExportPipeline.Run(new PipelineArgs());
    }
  }
}