namespace Sitecore.MediaFramework.Pipelines.MediaCleanup
{
  using System.Collections.Generic;
  using Sitecore.Data.Items;
  using Sitecore.Pipelines;

  public class MediaCleanupArgs : PipelineArgs
  {
    public string CleanupExecuterName { get; set; }

    public Item AccountItem { get; set; }

    public IList<Item> Items { get; set; }
  }
}