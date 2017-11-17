namespace Sitecore.MediaFramework.Pipelines.MediaCleanup
{
  using System.Linq;

  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Cleanup;

  public class ResolveScope : MediaCleanupProcessorBase
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
      Assert.ArgumentNotNull(args.AccountItem, "args.AccountItem");
      Assert.ArgumentNotNullOrEmpty(args.CleanupExecuterName, "args.CleanupExecuterName");

      ICleanupExecuter executer = MediaFrameworkContext.GetCleanupExecuter(args.CleanupExecuterName);
      if (executer != null)
      {
        args.Items = executer.GetScopeItems(args.AccountItem).ToList();
      }

      if (args.Items == null || args.Items.Count == 0)
      {
        args.AbortPipeline();
      }
    }
  }
}