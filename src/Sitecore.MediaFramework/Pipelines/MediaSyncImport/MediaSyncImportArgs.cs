namespace Sitecore.MediaFramework.Pipelines.MediaSyncImport
{
  using System.Collections.Generic;

  using Sitecore.Data.Items;
  using Sitecore.Pipelines;

  public class MediaSyncImportArgs : PipelineArgs
  {
    public Item AccountItem { get; set; }

    public string ImportName { get; set; }

    public object Body { get; set; }
    /// <summary>
    /// Gets or sets the data.
    /// </summary>
    public IEnumerable<object> ResultData { get; set; }
  }
}