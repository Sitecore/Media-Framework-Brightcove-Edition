namespace Sitecore.MediaFramework.Commands
{
  using System;                    
  using Sitecore.Data;        
  using Sitecore.Links;
  using Sitecore.MediaFramework.Diagnostics;
  using Sitecore.Shell.Framework;
  using Sitecore.Shell.Framework.Commands;
  using Sitecore.Text;
  using System.Linq;

  [Serializable]                                                
  public class OpenUploader : Command
  {
    public override void Execute(CommandContext context)
    {
      var app = Database.GetDatabase("core").GetItem(ItemIDs.UploadApplication);

      var url = new UrlString(LinkManager.GetItemUrl(app));
      var selectedItem = context.Items.FirstOrDefault();
      if (selectedItem != null)
      {
        url.Parameters.Add("itemId", selectedItem.ID.Guid.ToString());
        url.Parameters.Add("database", selectedItem.Database.Name);
        url.Parameters.Add("type", "norm");
      }

      try
      {
        Windows.RunApplication(app, app.Appearance.Icon, app.DisplayName, url.ToString());
      }
      catch (Exception ex)
      {
        LogHelper.Error("Opening Uploader failed.", this, ex);
      }
    }
    public override CommandState QueryState(CommandContext context)
    {
      return MediaFrameworkContext.IsExportAllowed() ? CommandState.Enabled : CommandState.Hidden;
    }
  }
}