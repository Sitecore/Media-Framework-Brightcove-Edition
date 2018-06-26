using System.Collections.Generic;
using System.Globalization;
using Brightcove.MediaFramework.Brightcove.Entities;
using Brightcove.MediaFramework.Brightcove.Extensions;
using RestSharp;
using Sitecore.RestSharp;
using Sitecore.RestSharp.Data;

namespace Brightcove.MediaFramework.Brightcove.Proxy.Security
{
    public class TokenProxy : BaseProxy
    {
        public TokenProxy()
            : base(null)
        {
        }

        protected override string ServiceName
        {
            get { return Constants.BrightCoveOauthService; }
        }

        public AccessToken RetrieveToken(string clientId, string clientSecret)
        {
            var salt = string.Format(CultureInfo.CurrentCulture, "Basic {0}", string.Format(CultureInfo.CurrentCulture, "{0}:{1}", clientId, clientSecret).ToBase64String());
            var context = new RestContext(Constants.BrightCoveOauthService);

            var parameters = new List<Parameter>{
                new Parameter {
                    Name = "grant_type",
                    Type = ParameterType.GetOrPost,
                    Value = "client_credentials"
                }
            };
            var request = context.CreateRequest<string, AccessToken>(EntityActionType.Create, "access_token", null, parameters)
                            .AddAuthorization(salt);

            var response = context.GetResponse<AccessToken>(request);
            return response.Data;
        }
    }
}