// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrightcovePlayerMarkupGenerator.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   The ooyala player markup provider.
// </summary>                                  
// ------------------------------------------------------------ --------------------------------------------------------

namespace Sitecore.MediaFramework.Brightcove.Players
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.Text;
  using System.Xml;

  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Account;
  using Sitecore.MediaFramework.Brightcove.Entities;
  using Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup;
  using Sitecore.MediaFramework.Players;
  using Sitecore.Xml;

  /// <summary>
  /// The ooyala player markup provider.
  /// </summary>
  public class BrightcovePlayerMarkupGenerator : PlayerMarkupGeneratorBase
  {
    public string ScriptUrl { get; set; }

    protected readonly Dictionary<string, string> DefaultParameters;

    public BrightcovePlayerMarkupGenerator()
    {
      this.DefaultParameters = new Dictionary<string, string>();
    }

    public void AddParameter(XmlNode configNode)
    {
      Assert.ArgumentNotNull(configNode, "configNode");

      string name = XmlUtil.GetAttribute("name", configNode);
      string value = XmlUtil.GetAttribute("value", configNode);

      if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
      {
        this.DefaultParameters[name] = value;
      }
    }

    /// <summary>
    /// Generate a player markup.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public override PlayerMarkupResult Generate(MediaGenerateMarkupArgs args)
    {
      var result = new PlayerMarkupResult();

      result.ScriptUrls.Add(this.ScriptUrl);
      result.ScriptUrls.Add(this.AnalyticsScriptUrl);

      var sb = new StringBuilder("<object class='BrightcoveExperience'>",1024);
      foreach (var pair in this.GetPlayerParameters(args))
      {
        sb.AppendLine("<param name='" + pair.Key + "' value='" + pair.Value + "' />");
      }
      sb.AppendLine("</object>");

      result.Html = sb.ToString();

      return result;
    }

    /// <summary>
    /// Generate a player markup.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public override string GetPreviewImage(MediaGenerateMarkupArgs args)
    {
      return PlayerManager.GetPreviewImage(args, FieldIDs.MediaElement.ThumbnailUrl);
    }

    public override Item GetDefaultPlayer(MediaGenerateMarkupArgs args)
    {
      ID fieldId = args.MediaItem.TemplateID == TemplateIDs.Video
                     ? FieldIDs.AccountSettings.DefaultVideoPlayer
                     : FieldIDs.AccountSettings.DefaultPlaylistPlayer;

      ReferenceField settingsField = AccountManager.GetSettingsField(args.AccountItem, fieldId);

      return settingsField != null ? settingsField.TargetItem : null;
    }

    public override string GetMediaId(Item item)
    {
      return item[FieldIDs.MediaElement.Id];
    }

    protected virtual Dictionary<string, string> GetPlayerParameters(MediaGenerateMarkupArgs args)
    {
      var dictionary = new Dictionary<string, string>(this.DefaultParameters);

      dictionary["wmode"] = args.PlayerItem[FieldIDs.Player.WMode];
      dictionary["bgcolor"] = args.PlayerItem[FieldIDs.Player.BackgroundColor];
      dictionary["width"] = args.Properties.Width.ToString(CultureInfo.InvariantCulture);
      dictionary["height"] = args.Properties.Height.ToString(CultureInfo.InvariantCulture);
      dictionary["autoStart"] = args.PlayerItem[FieldIDs.Player.AutoStart] == "1" ? "true" : "false";

      string playerId = args.PlayerItem[FieldIDs.Player.Id];
      long tmp;

      if (long.TryParse(playerId, out tmp))
      {
        dictionary["playerID"] = playerId;
      }
      else
      {
        dictionary["playerKey"] = playerId;
      }

      dictionary[this.GetIdentificatorKey(args)] = args.MediaItem[FieldIDs.MediaElement.Id];

      return dictionary;
    }

    protected virtual string GetIdentificatorKey(MediaGenerateMarkupArgs args)
    {
      PlayerPlaylistType playlistType;
      Enum.TryParse(args.PlayerItem[FieldIDs.Player.PlaylistType], true, out playlistType);

      switch (playlistType)
      {
        case PlayerPlaylistType.Tabbed:
          return "@playlistTabs";
        case PlayerPlaylistType.VideoList:
          return "@videoList";
        case PlayerPlaylistType.ComboBox:
          return "@playlistCombo";
        default:
          return "@videoPlayer";
      }
    }
  }
}