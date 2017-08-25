using System.Collections.Generic;
using AgencyOasis.MediaFramework.Brightcove.Extensions;
using RestSharp;
using AgencyOasis.MediaFramework.Brightcove.Entities;

namespace AgencyOasis.MediaFramework.Brightcove.Proxy.CMS
{
    public class DynamicIngestProxy : BaseProxy
    {
        public DynamicIngestProxy(IAuthenticator authenticator)
            : base(authenticator)
        {
        }

        public IngestVideo Ingest(IngestVideo entity)
        {
            return this.Create("ingest", entity, request => AddParameters(request, entity.VideoId));
        }

        protected override string ServiceName
        {
            get { return Constants.BrightCoveDynamicIngestService; }
        }

        private void AddParameters(RestRequest request, string videoId)
        {
            request.AddVideoId(videoId);
        }
    }
}