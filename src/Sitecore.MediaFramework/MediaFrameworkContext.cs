// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaFrameworkContext.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   Defines the MediaFrameworkContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework
{
  using System;
  using System.Collections.Generic;

  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Analytics;                               
  using Sitecore.MediaFramework.Cleanup;
  using Sitecore.MediaFramework.Export;
  using Sitecore.MediaFramework.Import;
  using Sitecore.MediaFramework.Players;
  using Sitecore.MediaFramework.Scopes;
  using Sitecore.MediaFramework.Synchronize;
  using Sitecore.MediaFramework.Upload;

  public static class MediaFrameworkContext
  {        
    static MediaFrameworkContext()
    {
      int[] defaultPlayerSize = ReadSize(Settings.GetSetting("Sitecore.MediaFramework.DefaultPlayerSize"), new [] {400,300});
      int[] previewSize = ReadSize(Settings.GetSetting("Sitecore.MediaFramework.PreviewSize"), new[] { 600, 400 });

      DefaultPlayerSize.Width = defaultPlayerSize[0];
      DefaultPlayerSize.Height = defaultPlayerSize[1];

      PreviewSize.Width = previewSize[0];
      PreviewSize.Height = previewSize[1];
    }

    //Size class was not used for preventing System.Drawing reference
    public static class DefaultPlayerSize
    {
      public static int Width;
      public static int Height;
    }

    public static class PreviewSize
    {
      public static int Width;
      public static int Height;
    }

    public static readonly int ExportTimeout = Settings.GetIntSetting("Sitecore.MediaFramework.ExportTimeout", 20);

    public static readonly bool EnableMigration = Settings.GetBoolSetting("Sitecore.MediaFramework.EnableMigration", false);

    public static readonly bool EnableDebug = Settings.GetBoolSetting("Sitecore.MediaFramework.EnableDebug", true);

    public static readonly List<string> ExportDatabases = new List<string>();

    //public static readonly Dictionary<Type, IItemSynchronizer> ItemSynchronizers = new Dictionary<Type, IItemSynchronizer>();

    public static readonly Dictionary<Type, IItemSynchronizer> ItemSynchronizersByType = new Dictionary<Type, IItemSynchronizer>();

    public static readonly Dictionary<ID, IItemSynchronizer> ItemSynchronizersByTemplate = new Dictionary<ID, IItemSynchronizer>();

    public static readonly Dictionary<string, IImportExecuter> ImportExecuters = new Dictionary<string, IImportExecuter>();

    public static readonly Dictionary<ID, IExportExecuter> ExportExecuters = new Dictionary<ID, IExportExecuter>();

    public static readonly Dictionary<string, ICleanupExecuter> CleanupExecuters = new Dictionary<string, ICleanupExecuter>();

    public static readonly Dictionary<ID, ICleanupLinksExecuter> CleanupLinksExecuters = new Dictionary<ID, ICleanupLinksExecuter>();

    public static readonly Dictionary<ID, IPlayerMarkupGenerator> PlayerMarkupGenerators = new Dictionary<ID, IPlayerMarkupGenerator>();

    public static readonly Dictionary<ID, IEventTrigger> EventTriggers = new Dictionary<ID, IEventTrigger>();

    public static readonly Dictionary<ID, IUploadExecuter> UploadExecuters = new Dictionary<ID, IUploadExecuter>();

    public static readonly Dictionary<string, ScopeExecuteConfiguration> ScopeExecuteConfigurations = new Dictionary<string, ScopeExecuteConfiguration>();

    public static readonly List<Guid> MediaEvents = new List<Guid>();  

    /// <summary>
    /// Is export allowed for current database
    /// </summary>
    /// <returns>True if export is allowed</returns>
    public static bool IsExportAllowed()
    {
      var database = Context.ContentDatabase ?? Context.Database;
      return database != null && IsExportAllowed(database.Name);
    }

    /// <summary>
    /// Is export allowed for selected database
    /// </summary>
    /// <param name="database">Database name</param>
    /// <returns>True if export is allowed</returns>
    public static bool IsExportAllowed(string database)
    {
      return ExportDatabases.Contains(database);
    }

    /// <summary>
    /// Resolve item synchronizer
    /// </summary>
    /// <param name="entity">Entity object</param>
    /// <returns>Item synchronizer</returns>
    public static IItemSynchronizer GetItemSynchronizer(object entity)
    {
      if (entity == null)
      {
        return null;
      }

      return GetByKey(ItemSynchronizersByType, entity.GetType());
    }

    /// <summary>
    /// Resolve item synchronizer
    /// </summary>
    /// <param name="entityType">Entity type</param>
    /// <returns>Item synchronizer</returns>
    public static IItemSynchronizer GetItemSynchronizer(Type entityType)
    {
      return GetByKey(ItemSynchronizersByType, entityType);
    }

    /// <summary>
    /// Resolve item synchronizer
    /// </summary>
    /// <param name="mediaItem">Media item template ID</param>
    /// <returns>Item synchronizer</returns>
    public static IItemSynchronizer GetItemSynchronizer(Item mediaItem)
    {
      if (mediaItem == null)
      {
        return null;
      }

      return GetItemSynchronizer(mediaItem.TemplateID);
    }

    /// <summary>
    /// Resolve item synchronizer
    /// </summary>
    /// <param name="mediaTemplate">Media item template ID</param>
    /// <returns>Item synchronizer</returns>
    public static IItemSynchronizer GetItemSynchronizer(ID mediaTemplate)
    {
      return GetByKey(ItemSynchronizersByTemplate, mediaTemplate);
    }

    /// <summary>
    /// Resolve import executer
    /// </summary>
    /// <param name="importName">Import name</param>
    /// <returns>Import executer</returns>
    public static IImportExecuter GetImportExecuter(string importName)
    {
      return GetByKey(ImportExecuters, importName);
    }

    /// <summary>
    /// Resolve export executer
    /// </summary>
    /// <param name="mediaItem">Media item</param>
    /// <returns>Export executer</returns>
    public static IExportExecuter GetExportExecuter(Item mediaItem)
    {
      if (mediaItem == null || mediaItem.Name == Sitecore.Constants.StandardValuesItemName)
      {
        return null;
      }

      return GetByKey(ExportExecuters, mediaItem.TemplateID);
    }

    /// <summary>
    /// Resolve export executer
    /// </summary>
    /// <param name="mediaTemplate">Media item template ID</param>
    /// <returns>Export executer</returns>
    public static IExportExecuter GetExportExecuter(ID mediaTemplate)
    {
      return GetByKey(ExportExecuters, mediaTemplate);
    }

    /// <summary>
    /// Resolve cleanup executer
    /// </summary>
    /// <param name="cleanupName">Cleanup name</param>
    /// <returns>Cleanup executer</returns>
    public static ICleanupExecuter GetCleanupExecuter(string cleanupName)
    {
      return GetByKey(CleanupExecuters, cleanupName);
    }

    /// <summary>
    /// Resolve cleanup links executer
    /// </summary>
    /// <param name="mediaItem">Media item</param>
    /// <returns>Cleanup links executer</returns>
    public static ICleanupLinksExecuter GetCleanupLinksExecuter(Item mediaItem)
    {
      if (mediaItem == null)
      {
        return null;
      }

      return GetCleanupLinksExecuter(mediaItem.TemplateID);
    }

    /// <summary>
    /// Resolve cleanup links executer
    /// </summary>
    /// <param name="mediaTemplate">Media item template ID</param>
    /// <returns>Cleanup links executer</returns>
    public static ICleanupLinksExecuter GetCleanupLinksExecuter(ID mediaTemplate)
    {
      return GetByKey(CleanupLinksExecuters, mediaTemplate);
    }

    /// <summary>
    /// Resolve player markup generator
    /// </summary>
    /// <param name="mediaItem">Media item</param>
    /// <returns>Player markup generator</returns>
    public static IPlayerMarkupGenerator GetPlayerMarkupGenerator(Item mediaItem)
    {
      if (mediaItem == null)
      {
        return null;
      }

      return GetPlayerMarkupGenerator(mediaItem.TemplateID);
    }

    /// <summary>
    /// Resolve player markup generator
    /// </summary>
    /// <param name="mediaTemplate">Media item template ID</param>
    /// <returns>Player markup generator</returns>
    public static IPlayerMarkupGenerator GetPlayerMarkupGenerator(ID mediaTemplate)
    {
      return GetByKey(PlayerMarkupGenerators, mediaTemplate);
    }

    /// <summary>
    /// Resolve scope execute configuration
    /// </summary>
    /// <param name="scopeName">Scope name</param>
    /// <returns>Scope execute configuration</returns>
    public static ScopeExecuteConfiguration GetScopeExecuteConfiguration(string scopeName)
    {
      return GetByKey(ScopeExecuteConfigurations, scopeName);
    }

    /// <summary>
    /// Resolve events trigger
    /// </summary>
    /// <param name="mediaItem">Media item</param>
    /// <returns>Event trigger</returns>
    public static IEventTrigger GetEventTrigger(Item mediaItem)
    {
      if (mediaItem == null)
      {
        return null;
      }

      return GetEventTrigger(mediaItem.TemplateID);
    }

    /// <summary>
    /// Resolve events trigger
    /// </summary>
    /// <param name="mediaTemplate">Media item template ID</param>
    /// <returns>Event trigger</returns>
    public static IEventTrigger GetEventTrigger(ID mediaTemplate)
    {
      return GetByKey(EventTriggers, mediaTemplate);
    }


    /// <summary>
    /// Resolve upload executer
    /// </summary>
    /// <param name="accountItem">Account item</param>
    /// <returns>Upload executer</returns>
    public static IUploadExecuter GetUploadExecuter(Item accountItem)
    {
      if (accountItem == null)
      {
        return null;
      }

      return GetUploadExecuter(accountItem.TemplateID);
    }

    /// <summary>
    /// Resolve upload executer
    /// </summary>
    /// <param name="accountTemplate">Account item template ID</param>
    /// <returns>Upload executer</returns>
    public static IUploadExecuter GetUploadExecuter(ID accountTemplate)
    {
      return GetByKey(UploadExecuters, accountTemplate);
    }

    public static bool IsMediaEvent(Guid definitionId)
    {
      return MediaEvents.Contains(definitionId);
    }

    public static bool IsMediaEvent(ID definitionId)
    {
      Assert.ArgumentNotNull(definitionId, "definitionId");

      return IsMediaEvent(definitionId.Guid);
    }

    private static TValue GetByKey<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key)
    {
      if (!ReferenceEquals(key, null))
      {
        TValue generator;
        if (dictionary.TryGetValue(key, out generator))
        {
          return generator;
        }
      }

      return default(TValue);
    }

    private static int[] ReadSize(string value, int[] defaultValue)
    {
      if (defaultValue == null || defaultValue.Length != 2)
      {
        defaultValue = new[] { 400, 300 };
      }

      if (!string.IsNullOrEmpty(value))
      {
        string[] sizeArray = value.Split(new[] { ',', '|' }, StringSplitOptions.RemoveEmptyEntries);

        if (sizeArray.Length == 2)
        {
          return new[] { MainUtil.GetInt(sizeArray[0], defaultValue[0]), MainUtil.GetInt(sizeArray[1], defaultValue[1]) };
        }
      }

      return defaultValue;
    }
  }
}