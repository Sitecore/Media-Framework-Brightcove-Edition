using AgencyOasis.MediaFramework.Brightcove.Proxy.CMS;
using RestSharp;
using PlayList = AgencyOasis.MediaFramework.Brightcove.Entities.PlayList;

namespace AgencyOasis.MediaFramework.Brightcove.Import
{
    public class PlayListCollectionImporter : AgencyOasis.MediaFramework.Brightcove.Import.EntityCollectionImporter<PlayList>
    {
        public override AgencyOasis.MediaFramework.Brightcove.Entities.Collections.PagedCollection<PlayList> RetrieveList(int limit, int offset, IAuthenticator authenticator)
        {
            return new PlaylistProxy(authenticator).RetrieveList(limit, offset);
        }
    }
}
