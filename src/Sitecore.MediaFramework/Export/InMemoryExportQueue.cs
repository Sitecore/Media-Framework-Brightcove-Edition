namespace Sitecore.MediaFramework.Export
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;

  using Sitecore.Data;
  using Sitecore.Data.Items;

  public class InMemoryExportQueue : ExportQueueProvider
  {
    protected static readonly ReaderWriterLockSlim Sync = new ReaderWriterLockSlim();

    protected static readonly List<ExportOperation> ExportScope = new List<ExportOperation>();

    public override void Add(ExportOperation operation)
    {
      Sync.EnterWriteLock();
      try
      {
        ExportScope.RemoveAll(i => i.Item.ID == operation.Item.ID);
        ExportScope.Add(operation);
        base.Add(operation);
      }
      finally
      {
        Sync.ExitWriteLock();
      }
    }

    public override bool IsExist(Func<ExportOperation, bool> predicate)
    {
      bool result;
      Sync.EnterReadLock();
      try
      {
        result = ExportScope.Any(predicate);
      }
      finally
      {
        Sync.ExitReadLock();
      }

      return result;
    }

    public override bool IsExist(ExportOperationType type, Item accountItem, ID fieldId, string fieldValue)
    {
      bool result;
      Sync.EnterReadLock();
      try
      {
        result =
          ExportScope.Any(
            op =>
            (op.Type & type) == op.Type && op.AccountItem.ID == accountItem.ID
            && op.AccountItem.Database.Name == accountItem.Database.Name && op.Item[fieldId] == fieldValue);
      }
      finally
      {
        Sync.ExitReadLock();
      }

      return result;
    }

    public override ExportOperation Get(Func<ExportOperation, bool> predicate)
    {
      ExportOperation result;
      Sync.EnterReadLock();
      try
      {
        result = ExportScope.FirstOrDefault(predicate);
      }
      finally
      {
        Sync.ExitReadLock();
      }

      return result;
    }

    public override List<ExportOperation> GetList(Func<ExportOperation, bool> predicate)
    {
      List<ExportOperation> result = new List<ExportOperation>();
      Sync.EnterReadLock();
      try
      {
        result.AddRange(ExportScope.Where(predicate));
      }
      finally
      {
        Sync.ExitReadLock();
      }

      return result;
    }


    public override List<ExportOperation> GetAll()
    {
      List<ExportOperation> result = new List<ExportOperation>();
      Sync.EnterReadLock();
      try
      {
        result.AddRange(ExportScope);
      }
      finally
      {
        Sync.ExitReadLock();
      }

      return result;
    }


    public override void Remove(ExportOperation operation)
    {
      Sync.EnterWriteLock();
      try
      {
        ExportScope.Remove(operation);
      }
      finally
      {
        Sync.ExitWriteLock();
      }
    }
  }
}