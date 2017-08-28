using Brightcove.MediaFramework.Brightcove.Proxy.CMS;
using RestSharp;
using PlayList = Brightcove.MediaFramework.Brightcove.Entities.PlayList;

namespace Brightcove.MediaFramework.Brightcove.Import
{
    /// <summary>
    /// PlayList Collection Importer
    /// </summary>
    public class PlayListCollectionImporter : Import.EntityCollectionImporter<PlayList>
    {
        public override Entities.Collections.PagedCollection<PlayList> RetrieveList(int limit, int offset, IAuthenticator authenticator)
        {
            return new PlaylistProxy(authenticator).RetrieveList(limit, offset);
        }
    }
}
