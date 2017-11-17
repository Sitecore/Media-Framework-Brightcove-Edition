namespace Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup
{
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Diagnostics;

  public class ResolveDatabase : MediaGenerateMarkupProcessorBase
  {
    public override void Process(MediaGenerateMarkupArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      if (args.MediaItem != null)
      {
        args.Database = args.MediaItem.Database;
      }
      else if(args.Database == null)
      {
        args.Database = Context.ContentDatabase ?? Context.Database;

        if (args.Database == null)
        {
          LogHelper.Warn("Database is null. Player markup generation will be stopped.", this);
          args.AbortPipeline();
        }
      }
    }
  }
}