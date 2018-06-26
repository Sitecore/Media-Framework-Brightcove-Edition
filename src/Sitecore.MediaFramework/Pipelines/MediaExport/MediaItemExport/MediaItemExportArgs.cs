namespace Sitecore.MediaFramework.Pipelines.MediaExport.MediaItemExport
{
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Export;
  using Sitecore.MediaFramework.Pipelines.Common;
  using Sitecore.Pipelines;

  public class MediaItemExportArgs : PipelineArgs, IItemArgs
  { 
    /// <summary>
    /// Gets or sets the data.
    /// </summary>
    public ExportOperation Operation { get; set; }

    public Item Item
    {
      get
      {
        return this.Operation != null ? this.Operation.Item : null;
      }
    }
  }
}