namespace Sitecore.MediaFramework.Commands
{
  using System;
  using System.Globalization;

  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup;
  using Sitecore.MediaFramework.Players;
  using Sitecore.MediaFramework.Utils;
  using Sitecore.Shell.Framework.Commands;
  using Sitecore.Text;
  using Sitecore.Web.UI.Sheer;

  [Serializable]
  public class MediaPreview : Command
  {
    public override void Execute(CommandContext context)
    {
      Assert.ArgumentNotNull(context, "context");

      Item item = this.GetItem(context);

      if (item != null)
      {
        var properties = this.GetPlayerProperties(context);

        var args = new MediaGenerateMarkupArgs
        {
          MarkupType = MarkupType.FrameUrl,
          MediaItem = item,
          Properties = properties,
        };

        MediaGenerateMarkupPipeline.Run(args);

        if (!args.Aborted && !string.IsNullOrEmpty(args.Result.Html))
        {
          UrlString url = new UrlString(args.Result.Html);
          url[Constants.PlayerParameters.ForceRender] = "1";

          url["sc_content"] = args.Database.Name;

          SheerResponse.ShowModalDialog(url.ToString(), properties.Width.ToString(CultureInfo.InvariantCulture), properties.Height.ToString(CultureInfo.InvariantCulture), string.Empty, false);
        }
        else
        {
          SheerResponse.Alert(Translations.MediaPreviewCouldNotBeShown);
        }
      } 
    }

    public override CommandState QueryState(CommandContext context)
    {
      Item item = this.GetItem(context);
      if (item != null && MediaItemUtil.IsMediaElement(item.Template))
      {
        return CommandState.Enabled;
      }
      return CommandState.Hidden;
    }

    protected virtual Item GetItem(CommandContext context)
    {
      if ((context.Items.Length > 0) && (context.Items[0] != null))
      {
        return context.Items[0];
      }
      
      string id = context.Parameters["id"];
      if (!string.IsNullOrEmpty(id))
      {
        return (Context.ContentDatabase ?? Context.Database).GetItem(id);
      }

      return null;
    }

    protected virtual PlayerProperties GetPlayerProperties(CommandContext context)
    {
      return new PlayerProperties
        {
          Width = MediaFrameworkContext.PreviewSize.Width,
          Height = MediaFrameworkContext.PreviewSize.Height
        };
    }
  }
}