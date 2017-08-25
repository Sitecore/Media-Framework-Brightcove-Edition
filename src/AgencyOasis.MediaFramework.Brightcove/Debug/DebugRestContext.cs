using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Sitecore.Configuration;
using Sitecore.RestSharp.Caching;
using Sitecore.RestSharp.Data;
using Sitecore.RestSharp.Service;
using RestSharp.Deserializers;

namespace AgencyOasis.MediaFramework.Brightcove.Debug
{
    public class DebugRestContext
    {
        protected static readonly ICache Cache;

        protected IServiceConfiguration Service { get; set; }

        public IRestClient Client { get; protected set; }

        static DebugRestContext()
        {
            string setting = Settings.GetSetting("Sitecore.RestSharp.Cache");
            if (setting.Length > 0)
                DebugRestContext.Cache = Activator.CreateInstance(Type.GetType(setting, true, false)) as ICache;
            if (DebugRestContext.Cache != null)
                return;
            DebugRestContext.Cache = (ICache)new Sitecore.RestSharp.Caching.Cache();
        }

        public DebugRestContext(string serviceName, IAuthenticator authenticator = null)
            : this(DebugRestContext.Cache.GetServiceConfiguration(serviceName), authenticator)
        {
        }

        public DebugRestContext(IServiceConfiguration service, IAuthenticator authenticator = null)
        {
            this.Service = service;
            this.Client = this.CreateClient();
            this.Client.Authenticator = authenticator ?? this.Service.Authenticator;
        }

        public IRestResponse<TResult> Create<TSource, TResult>(TSource body = null, IList<Parameter> parameters = null)
            where TSource : class
            where TResult : class, new()
        {
            return this.Create<TSource, TResult>((string)null, body, parameters);
        }

        public IRestResponse<TResult> Create<TSource, TResult>(string requestName, TSource body = null, IList<Parameter> parameters = null)
            where TSource : class
            where TResult : class, new()
        {
            return this.Service.RequestProvider.GetResult<TSource, TResult>(this.Service, this.Client, EntityActionType.Create, requestName, body, parameters);
        }

        public IRestResponse<TResult> Read<TSource, TResult>(TSource body = null, IList<Parameter> parameters = null)
            where TSource : class
            where TResult : class, new()
        {
            return this.Read<TSource, TResult>((string)null, body, parameters);
        }

        public IRestResponse<TResult> Read<TSource, TResult>(string requestName, TSource body = null, IList<Parameter> parameters = null)
            where TSource : class
            where TResult : class, new()
        {
            return this.Service.RequestProvider.GetResult<TSource, TResult>(this.Service, this.Client, EntityActionType.Read, requestName, body, parameters);
        }

        public IRestResponse<TResult> Read<TResult>(IList<Parameter> parameters = null) where TResult : class, new()
        {
            return this.Read<RestEmptyType, TResult>((string)null, (RestEmptyType)null, parameters);
        }

        public IRestResponse<TResult> Read<TResult>(string requestName, IList<Parameter> parameters = null) where TResult : class, new()
        {
            return this.Read<RestEmptyType, TResult>(requestName, (RestEmptyType)null, parameters);
        }

        public IRestResponse<TResult> Update<TSource, TResult>(TSource body = null, IList<Parameter> parameters = null)
            where TSource : class
            where TResult : class, new()
        {
            return this.Update<TSource, TResult>((string)null, body, parameters);
        }

        public IRestResponse<TResult> Update<TSource, TResult>(string requestName, TSource body = null, IList<Parameter> parameters = null)
            where TSource : class
            where TResult : class, new()
        {
            return this.Service.RequestProvider.GetResult<TSource, TResult>(this.Service, this.Client, EntityActionType.Update, requestName, body, parameters);
        }

        public IRestResponse<TResult> Delete<TSource, TResult>(TSource body = null, IList<Parameter> parameters = null)
            where TSource : class
            where TResult : class, new()
        {
            return this.Delete<TSource, TResult>((string)null, body, parameters);
        }

        public IRestResponse<TResult> Delete<TSource, TResult>(string requestName, TSource body = null, IList<Parameter> parameters = null)
            where TSource : class
            where TResult : class, new()
        {
            return this.Service.RequestProvider.GetResult<TSource, TResult>(this.Service, this.Client, EntityActionType.Delete, requestName, body, parameters);
        }

        public IRestResponse<TResult> Delete<TResult>(IList<Parameter> parameters = null) where TResult : class, new()
        {
            return this.Delete<RestEmptyType, TResult>((string)null, (RestEmptyType)null, parameters);
        }

        public IRestResponse<TResult> Delete<TResult>(string requestName, IList<Parameter> parameters = null) where TResult : class, new()
        {
            return this.Delete<RestEmptyType, TResult>(requestName, (RestEmptyType)null, parameters);
        }

        public IRestResponse<TResult> GetResult<TSource, TResult>(EntityActionType actionType, TSource body, IList<Parameter> parameters)
            where TSource : class
            where TResult : class, new()
        {
            return this.GetResult<TSource, TResult>(actionType, (string)null, body, parameters);
        }

        public IRestResponse<TResult> GetResult<TSource, TResult>(EntityActionType actionType, string requestName, TSource body, IList<Parameter> parameters)
            where TSource : class
            where TResult : class, new()
        {
            return this.Service.RequestProvider.GetResult<TSource, TResult>(this.Service, this.Client, actionType, requestName, body, parameters);
        }

        public IRestRequest CreateRequest<TSource, TResult>(EntityActionType actionType, string requestName, TSource body = null, IList<Parameter> parameters = null)
            where TSource : class
            where TResult : class, new()
        {
            return this.Service.RequestProvider.CreateRequest<TSource, TResult>(this.Service, actionType, requestName, body, parameters);
        }

        public IRestRequest CreateRequest<TSource, TResult>(EntityActionType actionType, TSource body = null, IList<Parameter> parameters = null)
            where TSource : class
            where TResult : class, new()
        {
            return this.CreateRequest<TSource, TResult>(actionType, (string)null, body, parameters);
        }

        public IRestResponse<TResult> GetResponse<TResult>(IRestRequest restRequest) where TResult : class, new()
        {
            return this.Service.RequestProvider.GetResponse<TResult>(this.Service, this.Client, restRequest);
        }

        protected virtual IRestClient CreateClient()
        {
            DebugRestClient restClient = new DebugRestClient(this.Service.BaseUrl);
            foreach (KeyValuePair<string, IDeserializer> keyValuePair in this.Service.Handlers)
                restClient.AddHandler(keyValuePair.Key, keyValuePair.Value);
            foreach (KeyValuePair<string, string> keyValuePair in this.Service.Headers)
                RestClientExtensions.AddDefaultHeader((IRestClient)restClient, keyValuePair.Key, keyValuePair.Value);
            return (IRestClient)restClient;
        }
    }
}