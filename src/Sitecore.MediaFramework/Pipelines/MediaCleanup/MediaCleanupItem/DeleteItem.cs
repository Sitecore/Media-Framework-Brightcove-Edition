namespace Sitecore.MediaFramework.Pipelines.MediaCleanup.MediaCleanupItem
{
  using System;

  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Diagnostics;

  public class DeleteItem : MediaCleanupItemProcessorBase
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

      try
      {
        if (args.Item.Access.CanDelete())
        {
          this.RemoveItem(args.Item);
        }
        else
        {
         args.AbortPipeline(); 
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error("Deleting Item Failed.", this, ex);
      }                                                         
    }

    protected virtual void RemoveItem(Item item)
    {
      ItemManager.DeleteItem(item);
    }
  }
}