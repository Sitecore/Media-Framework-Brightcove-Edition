using RestSharp;
using Brightcove.MediaFramework.Brightcove.Entities;

namespace Brightcove.MediaFramework.Brightcove.Proxy.CMS
{
    public class PlaylistProxy : BaseProxy
    {
        public PlaylistProxy(IAuthenticator authenticator)
            : base(authenticator)
        {
        }

        public Entities.Collections.PagedCollection<PlayList> RetrieveList(int limit, int offset)
        {
            return this.RetrieveList<PlayList>("read_playlists", limit, offset);
        }

        public PlayList Create(PlayList entity)
        {
            return this.Create<PlayList>("create_playlist", entity);
        }

        public PlayList Patch(PlayList entity)
        {
            var id = entity.Id;
            entity.Id = null;
            entity.Images = null;
            entity.CreationDate = null;
            entity.LastModifiedDate = null;
            entity.Favorite = null;
            return this.Patch<PlayList>("update_playlist", entity, "playlist_id", id);
        }

        public bool Delete(string id)
        {
            return this.Delete("delete_playlist", "playlist_id", id);
        }

        public PlayList RetrieveById(string id)
        {
            return this.RetrieveById<PlayList>("read_playlist_by_id", "playlist_id", id);
        }

        protected override string ServiceName
        {
            get { return Constants.BrightCoveCmsService; }
        }
    }
}