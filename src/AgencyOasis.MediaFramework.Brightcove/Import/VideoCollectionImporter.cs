using AgencyOasis.MediaFramework.Brightcove.Proxy.CMS;
using RestSharp;
using Video = AgencyOasis.MediaFramework.Brightcove.Entities.Video;

namespace AgencyOasis.MediaFramework.Brightcove.Import
{
    public class VideoCollectionImporter : AgencyOasis.MediaFramework.Brightcove.Import.EntityCollectionImporter<Video>
    {
        public override AgencyOasis.MediaFramework.Brightcove.Entities.Collections.PagedCollection<Video> RetrieveList(int limit, int offset, IAuthenticator authenticator)
        {
            return new VideoProxy(authenticator).RetrieveList(limit, offset);
        }
    }
}
