namespace Sitecore.MediaFramework.Pipelines.MediaCleanup.MediaCleanupItem
{
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Cleanup;

  public class CleanupLinks : MediaCleanupItemProcessorBase
  {
    /// <summary>
    /// Ingests items
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public override void Process(MediaCleanupItemArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.Item, "args.Item");

      ICleanupLinksExecuter executer = MediaFrameworkContext.GetCleanupLinksExecuter(args.Item);
      if (executer != null)
      {
        executer.CleanupLinks(args.Item);
      }                                                       
    }
  }
}