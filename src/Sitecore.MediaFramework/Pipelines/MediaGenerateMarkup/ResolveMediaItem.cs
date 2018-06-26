namespace Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup
{
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Diagnostics;

  public class ResolveMediaItem : MediaGenerateMarkupProcessorBase
  {
    public override void Process(MediaGenerateMarkupArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.Database, "args.Database");
      Assert.ArgumentNotNull(args.Properties, "args.PlayerProperties");

      if (args.MediaItem == null)
      {
        args.MediaItem = args.Database.GetItem(args.Properties.ItemId);
      }

      if (args.MediaItem != null)
      {
        args.Properties.ItemId = args.MediaItem.ID;
        
        if (args.MarkupType == MarkupType.Html)
        {
          args.Properties.Template = args.MediaItem.TemplateID;
        }
      }
      else
      {
        LogHelper.Warn("Media item could not be found. Player markup generation will be stopped.", this);

        args.AbortPipeline();
      }
    }
  }
}