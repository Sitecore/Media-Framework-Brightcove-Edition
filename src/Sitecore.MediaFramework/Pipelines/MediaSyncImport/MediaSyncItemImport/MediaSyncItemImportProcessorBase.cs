namespace Sitecore.MediaFramework.Pipelines.MediaSyncImport.MediaSyncItemImport
{
  using System.Collections.Generic;

  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Diagnostics;
  using Sitecore.Security.AccessControl;

  public abstract class MediaSyncItemImportProcessorBase
  {
    /// <summary>
    /// Ingests items
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public abstract void Process(MediaSyncItemImportArgs args);

    protected virtual bool CheckEdit(Item item)
    {
      return this.CheckItem(item, Context.User, new List<AccessRight> { AccessRight.ItemWrite, AccessRight.ItemRename });
    }

    protected virtual bool CheckCreate(Item item)
    {
      return this.CheckItem(item, Context.User, new List<AccessRight> { AccessRight.ItemCreate });
    }

    protected virtual bool CheckItem(Item item, Security.Accounts.Account account, List<AccessRight> accessRights)
    {
      if (item == null)
      {
        LogHelper.Debug("Item checking failed. Item is null", this);

        return false;
      }

      foreach (var accessRight in accessRights)
      {
        if (!AuthorizationManager.IsAllowed(item, accessRight, account))
        {
          LogHelper.Debug(string.Format("Item checking failed. Item:{0}, AccessRight:{1}", item.Uri, accessRight), this);

          return false;
        }
      }

      return true;
    }

    protected virtual bool ValidateActivity(MediaSyncItemImportArgs args, SyncAllowActivity allowActivity)
    {
      if ((args.SyncAllowActivity & allowActivity) != allowActivity)
      {
        LogHelper.Debug(string.Format("{0} is not enabled. Entity:{1}", allowActivity, args.Entity), this);
        return false;
      }

      return true;
    }
  }
}