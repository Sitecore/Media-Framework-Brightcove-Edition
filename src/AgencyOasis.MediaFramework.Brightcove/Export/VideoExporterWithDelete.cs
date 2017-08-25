using System.Collections.Specialized;
using System.Xml;
using Brightcove.MediaFramework.Brightcove.Entities;
using Brightcove.MediaFramework.Brightcove.Proxy.CMS;
using Brightcove.MediaFramework.Brightcove.Security;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.MediaFramework;
using Sitecore.MediaFramework.Export;
using Sitecore.MediaFramework.Synchronize;

namespace Brightcove.MediaFramework.Brightcove.Export
{
    public class VideoExporterWithDelete : VideoExporter, IConstructable
    {
        protected NameValueCollection DeleteParameters = new NameValueCollection();

        public string DeleteParams { get; set; }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="operation"></param>
        protected override void Delete(ExportOperation operation)
        {
            IItemSynchronizer itemSynchronizer = MediaFrameworkContext.GetItemSynchronizer(operation.Item);
            if (itemSynchronizer == null)
                return;
            var authenticator = new BrightcoveAuthenticator(operation.AccountItem);
            new VideoProxy(authenticator).Delete(((Asset)itemSynchronizer.CreateEntity(operation.Item)).Id);
        }

        public virtual void Constructed(XmlNode configuration)
        {
            DeleteParameters = StringUtil.GetNameValues(DeleteParams);
        }
    }
}
