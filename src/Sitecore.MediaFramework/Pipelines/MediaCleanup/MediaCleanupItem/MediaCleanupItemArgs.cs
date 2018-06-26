namespace Sitecore.MediaFramework.Pipelines.MediaCleanup.MediaCleanupItem
{
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Pipelines.Common;
  using Sitecore.Pipelines;

  public class MediaCleanupItemArgs : PipelineArgs, IItemArgs
  {
    public Item AccountItem { get; set; }

    public Item Item { get; set; }
  }
}