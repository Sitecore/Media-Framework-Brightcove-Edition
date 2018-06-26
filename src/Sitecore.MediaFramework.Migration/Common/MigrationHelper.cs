
namespace Sitecore.MediaFramework.Migration.Common
{
  using System;
  using System.Collections.Specialized;

  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Sitecore.Diagnostics;
  using Sitecore.Links;
  using Sitecore.MediaFramework.Account;
  using Sitecore.MediaFramework.Diagnostics;
  using Sitecore.MediaFramework.Players;
  using Sitecore.MediaFramework.Synchronize;
  using Sitecore.MediaFramework.Utils;
  using Sitecore.SecurityModel;

  public class MigrationHelper : IMigrationHelper
  {
    public virtual Item GetMediaItem(Database database, NameValueCollection collection)
    {
      return this.GetItem(database, collection, Constants.PlayerParameters.ItemId);
    }

    public virtual Item GetPlayerItem(Database database, NameValueCollection collection)
    {
      return this.GetItem(database, collection, Constants.PlayerParameters.PlayerId);
    }

    public virtual Item GetAccountItem(Item mediaItem)
    {
      Assert.ArgumentNotNull(mediaItem, "mediaItem");

      Item account = AccountManager.GetAccountItemForDescendant(mediaItem);

      if (account == null)
      {
        LogHelper.Warn("Account item could not be resolved. MediaItem:" + mediaItem.Uri, this);
      }

      return account;
    }

    public virtual IPlayerMarkupGenerator GetPlayerMarkupGenerator(Item mediaItem)
    {
      Assert.ArgumentNotNull(mediaItem, "mediaItem");

      IPlayerMarkupGenerator generator = MediaFrameworkContext.GetPlayerMarkupGenerator(mediaItem);
      if (generator == null)
      {
        LogHelper.Warn("Player markup generator could not be resolved. MediaItem:" + mediaItem.Uri, this);
      }
      return generator;
    }

    public virtual IItemSynchronizer GetItemSynchronizer(Item mediaItem)
    {
      Assert.ArgumentNotNull(mediaItem, "mediaItem");

      var synchronizer = MediaFrameworkContext.GetItemSynchronizer(mediaItem);
      if (synchronizer == null)
      {
        LogHelper.Warn("Item synchronizer could not be resolved. MediaItem:" + mediaItem.Uri, this);
      }
      return synchronizer;
    }

    public virtual object CreateEntity(Item mediaItem, IItemSynchronizer synchronizer = null)
    {
      Assert.ArgumentNotNull(mediaItem, "mediaItem");

      synchronizer = synchronizer ?? this.GetItemSynchronizer(mediaItem);

      if (synchronizer == null)
      {
        return null;
      }

      object entity = synchronizer.CreateEntity(mediaItem);
        
      if (entity == null)
      {
        LogHelper.Warn("Entity could not be resolved. MediaItem:" +  mediaItem.Uri, this);
      }
        
      return entity;
    }

    public virtual ID GetNewItemId(Item mediaItem)
    {
      Assert.ArgumentNotNull(mediaItem, "mediaItem");

      var synchronizer = this.GetItemSynchronizer(mediaItem);
      if (synchronizer == null)
      {
        return null;
      }

      object entity = this.CreateEntity(mediaItem, synchronizer);
      if (entity == null)
      {
        return null;
      }

      Item accountItem = this.GetAccountItem(mediaItem);
      if (accountItem == null)
      {
        return null;
      }

      var mediaData = synchronizer.GetMediaData(entity);
      if (mediaData == null)
      {
        return null;
      }

      return IdUtil.GenerateItemId(accountItem, mediaData);
    }

    public virtual Item RecreateItem(Item mediaItem, bool remove)
    {
      Assert.ArgumentNotNull(mediaItem, "mediaItem");

      if (!mediaItem.Access.CanDuplicate())
      {
        LogHelper.Warn("Not enough access to duplicate item. MediaItem:" +  mediaItem.Uri, this);
        return null;
      }

      if (!mediaItem.Access.CanDelete())
      {
        LogHelper.Warn("Not enough access to delete item. MediaItem:" + mediaItem.Uri, this);
        return null;
      }

      ID newItemId = this.GetNewItemId(mediaItem);

      if (ReferenceEquals(newItemId, null))
      {
        return null;
      }

      if (mediaItem.ID == newItemId)
      {
        return mediaItem;
      }

      Item newItem = null;

      try
      {
        Item item = mediaItem.Database.GetItem(newItemId);

        if (item != null)
        {
          if (item.TemplateID == mediaItem.TemplateID)
          {
            newItem = item;
          }
        }
        else
        {
          //CopyTo does not work with buckets
          newItem = ItemManager.AddFromTemplate(mediaItem.Name, mediaItem.TemplateID, mediaItem.Parent, newItemId);

          using (new EditContext(newItem,SecurityCheck.Disable))
          {
            foreach (Field field in mediaItem.Fields)
            {
              newItem[field.ID] = field.Value;
            }
          }

          LogHelper.Info(string.Format("Item has been recreated. Old Item:{0}; New Item:{1}", mediaItem.Uri, newItem.Uri), this);

          this.UpdateReferrers(mediaItem, newItem);
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error("New item could no be created. NewItemId:" + newItemId, this, ex);
      }

      if (remove)
      {
        try
        {
          mediaItem.Delete();
        }
        catch (Exception ex)
        {
          LogHelper.Error("Old item could not be deleted. Item:" + mediaItem.Uri, this, ex);
        }
      }

      return newItem;
    }

    public virtual void UpdateReferrers(Item oldItem, Item newItem)
    {
      Assert.ArgumentNotNull(oldItem, "oldItem");
      Assert.ArgumentNotNull(newItem, "newItem");

      try
      {
        var itemLinks = Sitecore.Globals.LinkDatabase.GetReferrers(oldItem);

        foreach (ItemLink itemLink in itemLinks)
        {
          if (itemLink.SourceFieldID == Sitecore.FieldIDs.Source)
          {
            continue;
          }

          Item sourceItem = itemLink.GetSourceItem();

          if (sourceItem == null)
          {
            continue;
          }

          foreach (Item item in sourceItem.Versions.GetVersions(true))
          {
            Field field = item.Fields[itemLink.SourceFieldID];

            CustomField customField = FieldTypeManager.GetField(field);

            if (customField != null)
            {
              using (new SecurityDisabler())
              {
                item.Editing.BeginEdit();
                customField.Relink(itemLink, newItem);
                item.Editing.EndEdit();
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(string.Format("Item links could not be updated. Old Item:{0}; New Item:{1}", oldItem.Uri, newItem.Uri), this, ex);
      }
    }

    protected virtual Item GetItem(Database database, NameValueCollection collection, string key)
    {
      Assert.ArgumentNotNull(database, "database");
      Assert.ArgumentNotNull(collection, "collection");
      Assert.ArgumentNotNull(key, "key");

      string idStr = collection[key];
      if (string.IsNullOrEmpty(idStr) || !ShortID.IsShortID(idStr))
      {
        LogHelper.Warn("Invalid id parameter:" + idStr, this);
        return null;
      }

      Item item = database.GetItem(new ID(idStr));

      if (item == null)
      {
        LogHelper.Warn("Item could not be found. ItemId:" + new ID(idStr), this);
      }
      return item;
    }
  }
}
