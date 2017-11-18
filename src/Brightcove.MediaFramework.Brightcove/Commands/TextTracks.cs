using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.MediaFramework;
using Sitecore.MediaFramework.Diagnostics;
using Sitecore.Shell.Framework;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.MediaFramework.Account;

namespace Brightcove.MediaFramework.Brightcove.Commands
{
    [Serializable]
    public class TextTracks : Command
    {
        public override void Execute(CommandContext context)
        {
            Item application = Database.GetDatabase("core").GetItem(new ID("{C4D485C4-ED0C-40EF-B576-A839AE62D00D}"));
            UrlString urlString = new UrlString(LinkManager.GetItemUrl(application));
            Item obj = Enumerable.FirstOrDefault<Item>((IEnumerable<Item>)context.Items);
            if (obj != null)
            {
                urlString.Parameters.Add("videoId", obj.Fields[FieldIDs.MediaElement.Id].Value);
                urlString.Parameters.Add("accountItemId", AccountManager.GetAccountItemForDescendant(Database.GetDatabase(obj.Database.Name).GetItem(obj.ID.ToString())).ID.ToString().Replace("{", "").Replace("}", ""));
                urlString.Parameters.Add("type", "norm");
            }
            try
            {
                Sitecore.Shell.Framework.Windows.RunApplication(application, application.Appearance.Icon, application.DisplayName, urlString.ToString());
            }
            catch (Exception ex)
            {
                LogHelper.Error("Opening Text Tracks failed.", (object)this, ex);
            }
        }

        protected virtual Item GetItem(CommandContext context)
        {
            if (context.Items.Length > 0 && context.Items[0] != null)
                return context.Items[0];
            string path = context.Parameters["id"];
            if (!string.IsNullOrEmpty(path))
                return (Sitecore.Context.ContentDatabase ?? Sitecore.Context.Database).GetItem(path);
            return (Item)null;
        }

        public override CommandState QueryState(CommandContext context)
        {
            Item obj = this.GetItem(context);
            return obj != null && obj.TemplateID.Equals(TemplateIDs.Video) ? CommandState.Enabled : CommandState.Hidden;
        }
    }
}
