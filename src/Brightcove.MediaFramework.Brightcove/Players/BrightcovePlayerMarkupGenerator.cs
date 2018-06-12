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

using Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup;
using Sitecore.MediaFramework.Players;
using Sitecore.StringExtensions;
using Sitecore.Xml;
using Sitecore.Configuration;
using System.Reflection;

// --------------------------------------------------------------------------------------------------------------------
// <summary>
//   The Brightcove player markup provider.
// </summary>                                  
// ------------------------------------------------------------ --------------------------------------------------------

namespace Brightcove.MediaFramework.Brightcove.Players
{
    /// <summary>
    /// The Brightcove player markup provider.
    /// </summary>
    public class BrightcovePlayerMarkupGenerator : PlayerMarkupGeneratorBase
    {
        protected readonly Dictionary<string, string> DefaultParameters;

        public string ScriptUrl { get; set; }
        private const string AccountToken = "{account_id}";
        private const string PlayerToken = "{player_id}";
        private const string ScriptTagTemplate = "<script type='text/javascript' src='{0}'></script>";

        public BrightcovePlayerMarkupGenerator()
        {
            this.DefaultParameters = new Dictionary<string, string>();
        }

        public void AddParameter(XmlNode configNode)
        {
            Assert.ArgumentNotNull((object)configNode, "configNode");
            string attribute1 = XmlUtil.GetAttribute("name", configNode);
            string attribute2 = XmlUtil.GetAttribute("value", configNode);
            if (string.IsNullOrEmpty(attribute1) || string.IsNullOrEmpty(attribute2))
                return;
            this.DefaultParameters[attribute1] = attribute2;
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
            PlayerMarkupResult playerMarkupResult = new PlayerMarkupResult();
            var scriptUrl = this.ScriptUrl;
            playerMarkupResult.ScriptUrls.Add(this.AnalyticsScriptUrl);
            StringBuilder stringBuilder = new StringBuilder("<video controls", 1024);

            foreach (KeyValuePair<string, string> keyValuePair in this.GetPlayerParameters(args, ref scriptUrl))
                stringBuilder.Append(" " + keyValuePair.Key + "='" + keyValuePair.Value + "'");
            stringBuilder.Append("></video>");

            if (!args.MediaItem.TemplateID.Equals(TemplateIDs.Video) && args.PlayerItem[FieldIDs.Player.ShowPlaylist] == "1")
            {
                stringBuilder.Append("<ol class='vjs-playlist'></ol>");
            }

            playerMarkupResult.Html = stringBuilder.ToString();
            AddPlayerScriptUrl(playerMarkupResult, args);
            
            return playerMarkupResult;
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
            ID fieldId = args.MediaItem.TemplateID == TemplateIDs.Video ? FieldIDs.AccountSettings.DefaultVideoPlayer : FieldIDs.AccountSettings.DefaultPlaylistPlayer;
            ReferenceField referenceField = (ReferenceField)AccountManager.GetSettingsField(args.AccountItem, fieldId);
            if (referenceField == null)
                return (Item)null;
            return referenceField.TargetItem;
        }

        public override string GetMediaId(Item item)
        {
            return item[FieldIDs.MediaElement.Id];
        }

        protected virtual Dictionary<string, string> GetPlayerParameters(MediaGenerateMarkupArgs args, ref string scriptUrl)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>((IDictionary<string, string>)this.DefaultParameters);

            // Add width and height attribute for player
            dictionary["style"] = "width:" +
                                  args.Properties.Width.ToString((IFormatProvider)CultureInfo.InvariantCulture) + "px" +
                                  ";height:" +
                                  args.Properties.Height.ToString((IFormatProvider)CultureInfo.InvariantCulture) + "px";

            // Add class attribute for player
            if (args.PlayerItem != null)
            {
                if (!args.PlayerItem[FieldIDs.Player.Class].IsNullOrEmpty())
                    dictionary["class"] = "video-js " + args.PlayerItem[FieldIDs.Player.Class];
                else
                    dictionary["class"] = "video-js";
            }

            // Set autoplay for player
            if (args.PlayerItem != null && args.PlayerItem[FieldIDs.Player.AutoStart] == "1")
            {
                dictionary["autoplay"] = args.PlayerItem[FieldIDs.Player.AutoStart];
            }

            // Set player id for player
            if (args.PlayerItem != null && !args.PlayerItem[FieldIDs.Player.Id].IsNullOrEmpty())
            {
                dictionary["data-player"] = args.PlayerItem[FieldIDs.Player.Id];
                scriptUrl = scriptUrl.Replace(PlayerToken, args.PlayerItem[FieldIDs.Player.Id]);
            }

            // Set account id for player
            if (args.AccountItem != null && !args.AccountItem[FieldIDs.Account.PublisherId].IsNullOrEmpty())
            {
                dictionary["data-account"] = args.AccountItem[FieldIDs.Account.PublisherId];
                scriptUrl = scriptUrl.Replace(AccountToken, args.AccountItem[FieldIDs.Account.PublisherId]);
            }

            // Set video/playlist id attribute for player
            if (args.MediaItem != null && !args.MediaItem[FieldIDs.MediaElement.Id].IsNullOrEmpty())
            {
                if (args.MediaItem.TemplateID.Equals(TemplateIDs.Video))
                {
                    dictionary["data-video-id"] = args.MediaItem[FieldIDs.MediaElement.Id];
                }
                else
                {
                    dictionary["data-playlist-id"] = args.MediaItem[FieldIDs.MediaElement.Id];
                }
            }
            //add cms version
            string sitecoreVersion = About.Version;
            string connectorVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            dictionary["data-usage"] = $"cms:sitecore:{sitecoreVersion}:{connectorVersion}:javascript";

            return dictionary;
        }

        private void AddPlayerScriptUrl(PlayerMarkupResult playerMarkupResult, MediaGenerateMarkupArgs args)
        {
            if (args.AccountItem != null && args.PlayerItem != null)
            {
                var publisherId = args.AccountItem[FieldIDs.Account.PublisherId];
                var playerId = args.PlayerItem[FieldIDs.Player.Id];

                if (!publisherId.IsNullOrEmpty() && !playerId.IsNullOrEmpty())
                {
                    var key = string.Format("{0}{1}Url", publisherId, playerId);

                    if (!playerMarkupResult.BottomScripts.ContainsKey(key))
                    {
                        var scriptUrl = ScriptUrl
                            .Replace(AccountToken, publisherId)
                            .Replace(PlayerToken, playerId);
                        var scriptTag = string.Format(ScriptTagTemplate, scriptUrl);
                        ////playerMarkupResult.BottomScripts.Add(key, scriptTag);
                        //var str = "PlayerEventsListener.prototype.playerScripts.push({key:\"" + key + "\", value:\"" +
                                  //scriptUrl + "\"});";
                        //playerMarkupResult.BottomScripts.Add(key, string.Format("{2}brightcoveListener.playerScripts.push({{key:\"{0}\", value:\"{1}\"}});{2}", key, scriptUrl, System.Environment.NewLine));

                        playerMarkupResult.BottomScripts.Add(key, string.Format("{2}brightcoveListener.playerScripts['{0}']='{1}';{2}", key, scriptUrl, System.Environment.NewLine));
                    }
                }
            }
        }
    }
}