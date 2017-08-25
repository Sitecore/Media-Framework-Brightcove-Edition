using System.Globalization;
using AgencyOasis.MediaFramework.Brightcove.Extensions;
using AgencyOasis.MediaFramework.Brightcove.Proxy.Security;
using RestSharp;
using Sitecore.Data.Items;

namespace AgencyOasis.MediaFramework.Brightcove.Security
{
    public class BrightcoveAuthenticator : IAuthenticator
    {
        public readonly string PublisherId;
        public readonly string ClientId;
        public readonly string ClientSecret;

        public BrightcoveAuthenticator(Item accountItem)
        {
            this.PublisherId = accountItem[Sitecore.MediaFramework.Brightcove.FieldIDs.Account.PublisherId];
            this.ClientId = accountItem[FieldIDs.Account.ClientId];
            this.ClientSecret = accountItem[FieldIDs.Account.ClientSecret];
        }

        public virtual void Authenticate(IRestClient client, IRestRequest request)
        {
            var tokenProxy = new TokenProxy();
            var token = tokenProxy.RetrieveToken(this.ClientId, this.ClientSecret);

            request.AddAuthorization(string.Format(CultureInfo.CurrentCulture, "{0} {1}", token.TokenType, token.Token));

            request.AddAccountId(this.PublisherId);
        }
    }
}
