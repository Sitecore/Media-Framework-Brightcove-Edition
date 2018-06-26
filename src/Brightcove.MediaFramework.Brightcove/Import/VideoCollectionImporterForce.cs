using Brightcove.MediaFramework.Brightcove.Proxy.CMS;
using RestSharp;
using System;
using System.Web;
using Video = Brightcove.MediaFramework.Brightcove.Entities.Video;

namespace Brightcove.MediaFramework.Brightcove.Import
{
    /// <summary>
    /// Video Collection Importer
    /// </summary>
    public class VideoCollectionImporterForce : Import.EntityCollectionImporter<Video>
    {
        /// <summary>
        /// Retrieve List
        /// </summary>
        public override Entities.Collections.PagedCollection<Video> RetrieveList(int limit, int offset, IAuthenticator authenticator)
        {
            HttpRuntime.Cache.Insert("BrightcoveForceSync", true, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60));
            return new VideoProxy(authenticator).RetrieveList(limit, offset);
        }
    }
}
