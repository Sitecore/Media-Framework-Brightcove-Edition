namespace Sitecore.MediaFramework.Pipelines.MediaCleanup
{
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Pipelines.MediaCleanup.MediaCleanupItem;

  public class CallItemCleanup : MediaCleanupProcessorBase
  {
    /// <summary>
    /// Ingests items
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public override void Process(MediaCleanupArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.Items, "args.Items");

      foreach (Item item in args.Items)
      {
        if (item == null)
        {
          continue;
        }

        MediaCleanupItemPipeline.Run(new MediaCleanupItemArgs { Item = item });
      } 
    }
  }
}