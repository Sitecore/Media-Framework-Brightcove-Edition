namespace Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup
{
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Diagnostics;

  public class ResolvePlayerItem : MediaGenerateMarkupProcessorBase
  {
    public override void Process(MediaGenerateMarkupArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.Generator, "args.Generator");

      if (args.PlayerItem == null)
      {
        args.PlayerItem = args.Database.GetItem(args.Properties.PlayerId);
        
        if (args.PlayerItem == null)
        {
          LogHelper.Warn("Selected player item could not be found. Default player will be used.", this);

          args.PlayerItem = args.Generator.GetDefaultPlayer(args);
        }
      }

      if (args.PlayerItem != null) 
      {
        args.Properties.PlayerId = args.PlayerItem.ID;
      }
      else
      {
        LogHelper.Warn("Player item could not be found. Player markup generation will be stopped.", this);

        args.AbortPipeline();
      }
    }
  }
}