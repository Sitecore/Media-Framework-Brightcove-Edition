

namespace Sitecore.MediaFramework.Export
{
  using System;
  using System.Collections.Generic;

  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Integration.Common.Providers;

  public static class ExportQueueManager
  {
    #region Initialization

    static ExportQueueManager()
    {
      var helper = new ProviderHelper<ExportQueueProvider, ProviderCollection<ExportQueueProvider>>("mediaFramework/exportQueueManager");

      Providers = helper.Providers;
      Provider = helper.Provider;
    }

    public static ExportQueueProvider Provider { get; set; }

    public static ProviderCollection<ExportQueueProvider> Providers { get; private set; }

    #endregion

    public static void Add(ExportOperation operation)
    {
      Provider.Add(operation);
    }

    public static bool IsExist(Func<ExportOperation, bool> predicate)
    {
      return Provider.IsExist(predicate);
    }

    public static bool IsExist(Item accountItem, ID fieldId, string fieldValue)
    {
      return Provider.IsExist(accountItem, fieldId, fieldValue);
    }

    public static bool IsExist(ExportOperationType type, Item accountItem, ID fieldId, string fieldValue)
    {
      return Provider.IsExist(type, accountItem, fieldId, fieldValue);
    }

    public static void Remove(ExportOperation operation)
    {
      Provider.Remove(operation);
    }

    public static ExportOperation Get(Func<ExportOperation, bool> predicate)
    {
      return Provider.Get(predicate);
    }

    public static List<ExportOperation> GetList(Func<ExportOperation, bool> predicate)
    {
      return Provider.GetList(predicate);
    }

    public static List<ExportOperation> GetAll()
    {
      return Provider.GetAll();
    }
  }
}