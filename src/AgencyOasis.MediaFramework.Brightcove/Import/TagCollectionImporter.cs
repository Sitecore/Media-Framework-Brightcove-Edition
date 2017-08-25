using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AgencyOasis.MediaFramework.Brightcove.Proxy.CMS;
using RestSharp;
using Sitecore.Data.Items;
using Sitecore.MediaFramework.Brightcove.Entities;
using Video = AgencyOasis.MediaFramework.Brightcove.Entities.Video;

namespace AgencyOasis.MediaFramework.Brightcove.Import
{
    public class TagCollectionImporter : AgencyOasis.MediaFramework.Brightcove.Import.EntityCollectionImporter<Video>
    {
        public override AgencyOasis.MediaFramework.Brightcove.Entities.Collections.PagedCollection<Video> RetrieveList(int limit, int offset, IAuthenticator authenticator)
        {
            return new VideoProxy(authenticator).RetrieveList(limit, offset);
        }

        public override IEnumerable<object> GetData(Item accountItem)
        {
            return (IEnumerable<object>)this.ReadAllTags(Enumerable.OfType<Video>((IEnumerable)base.GetData(accountItem)));
        }

        protected virtual IEnumerable<Tag> ReadAllTags(IEnumerable<Video> videoList)
        {
            foreach (Video video in videoList)
            {
                if (video.Tags != null && video.Tags.Count != 0)
                {
                    foreach (string str in video.Tags)
                        yield return new Tag()
                        {
                            Name = str
                        };
                }
            }
        }
    }
}
