using Brightcove.MediaFramework.Brightcove.Proxy.CMS;
using RestSharp;
using Video = Brightcove.MediaFramework.Brightcove.Entities.Video;

namespace Brightcove.MediaFramework.Brightcove.Import
{
    /// <summary>
    /// Video Collection Importer
    /// </summary>
    public class VideoCollectionImporter : Import.EntityCollectionImporter<Video>
    {
        /// <summary>
        /// Retrieve List
        /// </summary>
        public override Entities.Collections.PagedCollection<Video> RetrieveList(int limit, int offset, IAuthenticator authenticator)
        {
            return new VideoProxy(authenticator).RetrieveList(limit, offset);
        }
    }
}
