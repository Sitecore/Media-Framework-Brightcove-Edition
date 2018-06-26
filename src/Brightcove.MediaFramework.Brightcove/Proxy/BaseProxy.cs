using System;
using Brightcove.MediaFramework.Brightcove.Extensions;
using RestSharp;
using Sitecore.Configuration;
using Sitecore.RestSharp;
using Sitecore.Web.UI.HtmlControls;
using System.Text;
using Sitecore.Diagnostics;

namespace Brightcove.MediaFramework.Brightcove.Proxy
{
    public abstract class BaseProxy
    {
        private readonly IAuthenticator _authenticator;

        protected BaseProxy(IAuthenticator authenticator)
        {
            this._authenticator = authenticator;
        }

        protected abstract string ServiceName { get; }

        protected RestContext CreateContext()
        {
            return new RestContext(this.ServiceName, this._authenticator);
        }

        public Entities.Collections.PagedCollection<T> RetrieveList<T>(string requestName, int limit, int offset)
        {
            var context = this.CreateContext();
            var request = new RestRequest();
            request.AddOffset(offset)
                .AddLimit(limit);
            var response = context.Read<Entities.Collections.PagedCollection<T>>(requestName, request.Parameters);
            if (response != null && Configuration.Settings.EnableAdvancedLogging)
            {
                var hsb = new StringBuilder();
                foreach (var h in response.Headers)
                {
                    hsb.Append(h.Name + ":" + h.Value);
                }
                Log.Info("Brightcove response headers: " + hsb.ToString(), this);
                Log.Info("Brightcove reponse content: " + response.Content.ToString(), this);
            }
            return response == null ? null : response.Data;
        }

        public T Create<T>(string requestName, T entity) where T : class, new()
        {
            return this.Create(requestName, entity, null);
        }

        public T Create<T>(string requestName, T entity, Action<RestRequest> action) where T : class, new()
        {
            var context = this.CreateContext();
            var request = new RestRequest();
            if (action != null)
            {
                action(request);
            }

            var response = context.Create<T, T>(requestName, entity, request.Parameters);
            return response == null ? null : response.Data;
        }

        public T Patch<T>(string requestName, T entity, string idParameterName, string id) where T : class, new()
        {
            var context = this.CreateContext();
            var request = new RestRequest()
                .AddId(idParameterName, id);
            var response = context.Update<T, T>(requestName, entity, request.Parameters);
            return response == null ? null : response.Data;
        }

        public bool Delete(string requestName, string idParameterName, string id)
        {
            var context = this.CreateContext();
            var request = new RestRequest()
                .AddId(idParameterName, id); 
            var response = context.Delete<object>(requestName, request.Parameters);
            return response != null;
        }

        public T RetrieveById<T>(string requestName, string idParameterName, string id) where T : class, new()
        {
            var context = this.CreateContext();
            var request = new RestRequest()
                .AddId(idParameterName, id); 
            var response = context.Read<T>(requestName, request.Parameters);
            return response == null ? null : response.Data;
        }

        public T Retrieve<T>(string requestName) where T : class, new()
        {
            var context = this.CreateContext();
            var request = new RestRequest();
            var response = context.Read<T>(requestName, request.Parameters);
            return response == null ? null : response.Data;
        }
    }
}