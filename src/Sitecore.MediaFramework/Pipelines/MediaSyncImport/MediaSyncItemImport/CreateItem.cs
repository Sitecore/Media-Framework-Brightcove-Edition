namespace Sitecore.MediaFramework.Pipelines.MediaSyncImport.MediaSyncItemImport
{
  using System;

  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Diagnostics;
  using Sitecore.MediaFramework.Entities;
  using Sitecore.MediaFramework.Utils;

  public class CreateItem : MediaSyncItemImportProcessorBase
  {
    public override void Process(MediaSyncItemImportArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.Entity, "args.Entity");
      Assert.ArgumentNotNull(args.AccountItem, "args.AccountItem");
      Assert.ArgumentNotNull(args.Synchronizer, "args.Synchronizer");

      if (!this.ValidateActivity(args, SyncAllowActivity.CreateItem))
      {
        return;
      }

      if (args.SearchResultItem == null && args.Item == null)
      {
        Item rootItem = args.Synchronizer.GetRootItem(args.Entity, args.AccountItem);

        if (this.CheckCreate(rootItem))
        {
          var mediaData = args.Synchronizer.GetMediaData(args.Entity);

          string itemName = ItemUtil.ProposeValidItemName(mediaData.EntityName);

          ID itemId = this.GenerateItemId(args.AccountItem, mediaData);

          args.Item = this.Create(itemName, mediaData.TemplateId, rootItem, itemId);

          if (args.Item != null)
          {
            LogHelper.Debug(string.Format("Item has been created. Entity:{0}; Item: {1}", args.Entity, args.Item.Uri), this);
          }
          else
          {
            LogHelper.Debug("Item could not be created. Entity:" + args.Entity, this);
          }
        }
      }
    }

    protected virtual Item Create(string itemName, ID templateId, Item rootItem, ID itemId)
    {
      try
      {
        return ItemManager.AddFromTemplate(itemName, templateId, rootItem, itemId);
      }
      catch (Exception ex)
      {
        LogHelper.Error("Error during item creation. ItemId:" + itemId, this, ex);
      }
      return null;
    }

    protected virtual ID GenerateItemId(Item accountItem, MediaServiceEntityData mediaData)
    {
      return IdUtil.GenerateItemId(accountItem, mediaData);
    }
  }
}