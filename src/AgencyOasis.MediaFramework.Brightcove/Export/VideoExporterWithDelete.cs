using System.Collections.Specialized;
using System.Xml;
using AgencyOasis.MediaFramework.Brightcove.Entities;
using AgencyOasis.MediaFramework.Brightcove.Proxy.CMS;
using AgencyOasis.MediaFramework.Brightcove.Security;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.MediaFramework;
using Sitecore.MediaFramework.Export;
using Sitecore.MediaFramework.Synchronize;

namespace AgencyOasis.MediaFramework.Brightcove.Export
{
    public class VideoExporterWithDelete : AgencyOasis.MediaFramework.Brightcove.Export.VideoExporter, IConstructable
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
