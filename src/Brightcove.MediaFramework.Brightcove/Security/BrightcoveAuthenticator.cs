using System.Globalization;
using Brightcove.MediaFramework.Brightcove.Extensions;
using Brightcove.MediaFramework.Brightcove.Proxy.Security;
using RestSharp;
using Sitecore.Data.Items;

namespace Brightcove.MediaFramework.Brightcove.Security
{
    public class BrightcoveAuthenticator : IAuthenticator
    {
        public readonly string PublisherId;
        public readonly string ClientId;
        public readonly string ClientSecret;

        public BrightcoveAuthenticator(Item accountItem)
        {
            this.PublisherId = accountItem[FieldIDs.Account.PublisherId];
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
