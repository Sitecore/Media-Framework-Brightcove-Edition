// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InitializeContext.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   Initializes the context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Pipelines.Initialize
{
  using System;

  using Sitecore.Configuration;
  using Sitecore.Integration.Common.Pipelines.Initialize;
  using Sitecore.MediaFramework.Analytics;
  using Sitecore.Pipelines;

  /// <summary>
  /// Initializes the context.
  /// </summary>
  public class InitializeContext : InitializeContextBase
  {
    /// <summary>
    /// Ingests items
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public override void Process(PipelineArgs args)
    {
      this.InitExportDatabases();
      this.InitImportExecuters();
      this.InitExportExecuters();
      this.InitUploadExecuters();
      this.InitSynchronizers();
      this.InitCleanupExecuters();
      this.InitCleanupLinks();
      this.InitPlayerMarkupGenerators();
      this.InitScopeExecuteConfigurations();
      this.InitPlayerEventsTriggers();
      this.InitMediaEvents();
    }

    protected virtual void InitExportDatabases()
    {
      string value = Settings.GetSetting("Sitecore.MediaFramework.ExportDatabases");
      string[] databases = value.Split(new[] { '|', ',' }, StringSplitOptions.RemoveEmptyEntries);

      MediaFrameworkContext.ExportDatabases.AddRange(databases);
    }

    /// <summary>
    /// Initializes scope execute configurations.
    /// </summary>
    protected virtual void InitScopeExecuteConfigurations()
    {
      this.InitByStringMapper(MediaFrameworkContext.ScopeExecuteConfigurations, Constants.ConfigNodes.ScopeExecuteConfigurations, "name");
    }

    /// <summary>
    /// Initializes synchronizers collection.
    /// </summary>
    protected virtual void InitSynchronizers()
    {
      this.InitByTypeMapper(MediaFrameworkContext.ItemSynchronizersByType, Constants.ConfigNodes.ItemSynchronizers, "entity");

      this.InitByIdMapper(MediaFrameworkContext.ItemSynchronizersByTemplate, Constants.ConfigNodes.ItemSynchronizers, "templateId");
    }

    /// <summary>
    /// Initializes export executers collection.
    /// </summary>
    protected virtual void InitExportExecuters()
    {
      this.InitByIdMapper(MediaFrameworkContext.ExportExecuters, Constants.ConfigNodes.ExportExecuters, "templateId");
    }

    /// <summary>
    /// Initializes export executers collection.
    /// </summary>
    protected virtual void InitUploadExecuters()
    {
      this.InitByIdMapper(MediaFrameworkContext.UploadExecuters, Constants.ConfigNodes.UploadExecuters, "accountTemplate");
    }

    /// <summary>
    /// Initializes import executers collection.
    /// </summary>
    protected virtual void InitImportExecuters()
    {
      this.InitByStringMapper(MediaFrameworkContext.ImportExecuters, Constants.ConfigNodes.ImportExecuters, "name");
    }

    /// <summary>
    /// Initializes cleanup executers collection.
    /// </summary>
    protected virtual void InitCleanupExecuters()
    {
      this.InitByStringMapper(MediaFrameworkContext.CleanupExecuters, Constants.ConfigNodes.CleanupExecuters, "name");
    }
    
    /// <summary>
    /// Initializes cleanup links collection.
    /// </summary>
    protected virtual void InitCleanupLinks()
    {
      this.InitByIdMapper(MediaFrameworkContext.CleanupLinksExecuters, Constants.ConfigNodes.CleanupLinks, "templateId");
    }

    /// <summary>
    /// Initializes player markup generators collection.
    /// </summary>
    protected virtual void InitPlayerMarkupGenerators()
    {
      this.InitByIdMapper(MediaFrameworkContext.PlayerMarkupGenerators, Constants.ConfigNodes.PlayerMarkupGenerators, "templateId");
    }

    /// <summary>
    /// Initializes a player player events triggers.
    /// </summary>
    protected virtual void InitPlayerEventsTriggers()
    {
      this.InitByIdMapper(MediaFrameworkContext.EventTriggers, Constants.ConfigNodes.PlayerEventsTriggers, "templateId");
      
      foreach (IEventTrigger eventTrigger in MediaFrameworkContext.EventTriggers.Values)
      {
        eventTrigger.InitEvents();
      }
    }

    protected virtual void InitMediaEvents()
    {
      //TODO: from config
      MediaFrameworkContext.MediaEvents.Add(ItemIDs.PageEvents.PlaybackStarted.Guid);
      MediaFrameworkContext.MediaEvents.Add(ItemIDs.PageEvents.PlaybackCompleted.Guid);
      MediaFrameworkContext.MediaEvents.Add(ItemIDs.PageEvents.PlaybackChanged.Guid);
      MediaFrameworkContext.MediaEvents.Add(ItemIDs.PageEvents.PlaybackError.Guid);
    }
  }
}