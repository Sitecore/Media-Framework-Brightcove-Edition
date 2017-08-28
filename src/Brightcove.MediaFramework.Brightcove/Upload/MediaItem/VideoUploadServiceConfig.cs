////using Sitecore.Data;
////using Sitecore.Data.Items;

////namespace Brightcove.MediaFramework.Brightcove.Upload.MediaItem
////{
////    public class VideoUploadServiceConfig : VideoUploadServiceConfigBase
////    {
////        public string ContentDatabaseName { get; set; }
////        public string PublishDatabaseName { get; set; }

////        public Sitecore.Data.Database ContentDatabase
////        {
////            get { return Sitecore.Data.Database.GetDatabase(ContentDatabaseName); }
////        }

////        public Sitecore.Data.Database PublishDatabase
////        {
////            get { return Sitecore.Data.Database.GetDatabase(PublishDatabaseName); }
////        }

////        public TemplateItem MediaFolderTemplate
////        {
////            get { return ContentDatabase.GetTemplate(new ID("{FE5DD826-48C6-436D-B87A-7C4210C7413B}")); }
////        }

////        public Item RootItem
////        {
////            get { return ContentDatabase.GetItem(new ID("{7150B2B1-EB68-40EE-991F-2AFEDB532C46}")); }
////        }

////        public Item PublishStop
////        {
////            get { return ContentDatabase.GetItem(new ID("{3D6658D8-A0BF-4E75-B3E2-D050FABCF4E1}")); }
////        }
////    }
////}