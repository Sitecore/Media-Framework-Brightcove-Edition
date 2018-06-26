// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   The constants.                                                                                                                          
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework
{
  /// <summary>
  /// The constants.
  /// </summary>
  public class Constants
  {
    public static readonly string PlayerIframeUrl = "/layouts/MediaFramework/Sublayouts/Player.aspx";

    public static readonly string IdTablePrefix = "MediaFramework_scope";

    public static readonly string DefaultPreview = "/sitecore/shell/Themes/Standard/Images/MediaFramework/Common/Player.png";

    public static readonly string OkImage = "/sitecore/shell/Themes/Standard/Images/MediaFramework/Common/gal.png";

    public static readonly string ErrorImage = "/sitecore/shell/Themes/Standard/Images/MediaFramework/Common/stop.png";

    /// <summary>
    /// The configuration paths.
    /// </summary>
    public static class PlayerParameters
    {
      public static readonly string ItemId = "itemId";
      public static readonly string Template = "template";
      public static readonly string MediaId = "mediaId";
      public static readonly string PlayerId = "playerId";
      public static readonly string Width = "width";
      public static readonly string Height = "height";
      public static readonly string ForceRender = "forceRender";
      public static readonly string ActivePage = "active";
    }

    public static class PlayerEventParameters
    {
      public static readonly string MediaName = "mediaName";
      public static readonly string MediaLength = "mediaLength";
      public static readonly string EventParameter = "eventParameter";
      public static readonly string ContextItemId = "contextItemId";
    }

    public static class Events
    {
      public static readonly string EventName = "mediaframework:content:import:remote";   
    }

    public static class Upload
    {
      public static readonly string RequestType = "rt";
      public static readonly string Goto = "gt";
      public static readonly string Cancel = "cncl";
      public static readonly string UploadStatus = "upst";
      public static readonly string FileId = "fid";
      public static readonly string AccountId = "accId";
      public static readonly string MediaItemId = "mItemId";
      public static readonly string AccountTemplateId = "accTemplateId";
      public static readonly string FileName = "fname";
      public static readonly string Database = "db";
      public static readonly string StartUpload = "up";
      public static readonly string Progress = "pr";        
      public static readonly string Id = "id";        
    }

    /// <summary>
    /// The configuration paths.
    /// </summary>
    public static class ConfigNodes
    {
      /// <summary>
      /// The import executers cofig path.
      /// </summary>
      public static readonly string ImportExecuters = "mediaFramework/mediaImport/importExecuters/*";

      /// <summary>
      /// The export executers cofig path.
      /// </summary>
      public static readonly string ExportExecuters = "mediaFramework/mediaExport/exportExecuters/*";

      /// <summary>
      /// The import executers cofig path.
      /// </summary>
      public static readonly string UploadExecuters = "mediaFramework/mediaExport/uploadExecuters/*";

      /// <summary>
      /// The cleanup links cofig path.
      /// </summary>
      public static readonly string CleanupLinks = "mediaFramework/mediaCleanup/cleanupLinks/*";

      /// <summary>
      /// The cleanup executers cofig path.
      /// </summary>
      public static readonly string CleanupExecuters = "mediaFramework/mediaCleanup/cleanupExecuters/*";

      /// <summary>
      /// The item synchronizers cofig path.
      /// </summary>
      public static readonly string ItemSynchronizers = "mediaFramework/synchronizers/*";

      /// <summary>
      /// The player markup generators cofig path.
      /// </summary>
      public static readonly string PlayerMarkupGenerators = "mediaFramework/playerMarkupGenerators/*";

      /// <summary>
      /// The scope execute configurations cofig path.
      /// </summary>
      public static readonly string ScopeExecuteConfigurations = "mediaFramework/scopeExecuteConfigurations/*";

      /// <summary>
      /// The player events triggers cofig path.
      /// </summary>
      public static readonly string PlayerEventsTriggers = "mediaFramework/playerEventsTriggers/*";
    }
  }
}