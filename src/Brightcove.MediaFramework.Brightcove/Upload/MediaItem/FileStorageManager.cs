using Brightcove.MediaFramework.Brightcove.Entities;
using Brightcove.MediaFramework.Brightcove.Extensions;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using System;
using System.IO;

namespace Brightcove.MediaFramework.Brightcove.Upload.MediaItem
{
    public static class FileStorageManager
    {
        public static void Publish(Item itemToPublish, Sitecore.Data.Database publishDatabase)
        {
            var publishOptions =
  new Sitecore.Publishing.PublishOptions(itemToPublish.Database,
                                         publishDatabase,
                                         Sitecore.Publishing.PublishMode.SingleItem,
                                         itemToPublish.Language,
                                         DateTime.Now);
            var publisher = new Sitecore.Publishing.Publisher(publishOptions);
            publisher.Options.RootItem = itemToPublish;
            publisher.Options.Deep = true;
            publisher.Publish();
        }

        public static void DeleteMediaItem(Item root, string itemId, Sitecore.Data.Database contentDatabase, Sitecore.Data.Database publishDatabase)
        {
            var itemToDelete = contentDatabase.GetItem(new ID(itemId));

            if (itemToDelete != null && itemToDelete.Parent.ID == root.ID)
            {
                using (new SecurityDisabler())
                {
                    itemToDelete.Editing.BeginEdit();
                    itemToDelete.Publishing.NeverPublish = true;
                    itemToDelete.Editing.EndEdit();
                }

                Publish(itemToDelete, publishDatabase);

                using (new SecurityDisabler())
                {
                    itemToDelete.Delete();
                }
            }
        }

        public static Item VerifyRoot(Item root, TemplateItem template)
        {
            var parentItem = root;
            parentItem = VerifyChildItem(parentItem, template, "Temp");
            parentItem = VerifyChildItem(parentItem, template, "brightcove");
            parentItem = VerifyChildItem(parentItem, template, "files");
            return parentItem;
        }

        private static Item VerifyChildItem(Item parentItem, TemplateItem templateItem, string name)
        {
            var child = parentItem.Children[name];

            parentItem = child ?? CreateItem(parentItem, templateItem, name);

            return parentItem;
        }

        public static Item CreateItem(Item parentItem, TemplateItem templateItem, string name)
        {
            using (new SecurityDisabler())
            {
                var item = parentItem.Add(name, templateItem);
                return item;
            }
        }

        public static Sitecore.Data.Items.MediaItem AddFile(Sitecore.Data.Database database, UploadFileInfo uploadFileInfo, string destination)
        {
            var itemName = ItemUtil.ProposeValidItemName(uploadFileInfo.FileNameWithoutExtension);
            // Create the options
            var options = new Sitecore.Resources.Media.MediaCreatorOptions();
            // Store the file in the database, not as a file
            options.FileBased = false;
            // Remove file extension from item name
            options.IncludeExtensionInItemName = false;
            // Overwrite any existing file with the same name
            options.OverwriteExisting = true;
            // Do not make a versioned template
            options.Versioned = false;
            // set the path
            options.Destination = destination + "/" + itemName;
            // Set the database
            options.Database = database;

            using (var stream = new MemoryStream(uploadFileInfo.Bytes))
            {
                return Sitecore.Resources.Media.MediaManager.Creator.CreateFromStream(stream, uploadFileInfo.Name, options);
            }
        }

        public static Item ResolvePublishItem(Item item, Item stopItem, Sitecore.Data.Database publishDatabase)
        {
            if (item.Parent == null || item.Parent.ID == stopItem.ID || publishDatabase.GetItem(item.ID) != null)
            {
                return item;
            }

            return ResolvePublishItem(item.Parent, stopItem, publishDatabase);
        }

        public static UploadFileInfo RetrieveFile(string fileId, Sitecore.Data.Database database)
        {
            var item = database.GetItem(new ID(fileId));
            if (item != null)
            {
                var mediaItem = new Sitecore.Data.Items.MediaItem(item);

                return new UploadFileInfo
                {
                    Id = fileId,
                    Name = mediaItem.InnerItem.Name + "." + mediaItem.Extension,
                    Bytes = mediaItem.GetMediaStream().ToBytes(mediaItem.Size)
                };
            }

            return null;
        }
    }
}