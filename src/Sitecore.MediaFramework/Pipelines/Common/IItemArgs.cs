namespace Sitecore.MediaFramework.Pipelines.Common
{
  using Sitecore.Data.Items;

  /// <summary>
  /// Item Args interface
  /// </summary>
  public interface IItemArgs
  {
    /// <summary>
    /// Gets the item.
    /// </summary>
    Item Item { get; }
  }
}