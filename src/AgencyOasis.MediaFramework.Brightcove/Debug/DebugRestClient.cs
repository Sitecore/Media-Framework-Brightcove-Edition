// Type: RestSharp.DebugRestClient
// Assembly: RestSharp, Version=104.2.0.0, Culture=neutral
// MVID: 770CA01C-2D7C-4179-999E-41A3E85DE492
// Assembly location: D:\Projects\AgencyOasis\Bright Cove\2.2\Lib\Sitecore\RestSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Extensions;

namespace AgencyOasis.MediaFramework.Brightcove.Debug
{
    public class DebugRestClient : IRestClient
    {
        private static readonly Version version = new AssemblyName(Assembly.GetExecutingAssembly().FullName).Version;
        public IHttpFactory HttpFactory = (IHttpFactory)new SimpleFactory<Http>();
        private string _baseUrl;

        private IDictionary<string, IDeserializer> ContentHandlers { get; set; }

        private IList<string> AcceptTypes { get; set; }

        public IList<Parameter> DefaultParameters { get; private set; }

        public int? MaxRedirects { get; set; }

        public X509CertificateCollection ClientCertificates { get; set; }

        public IWebProxy Proxy { get; set; }

        public bool FollowRedirects { get; set; }

        public CookieContainer CookieContainer { get; set; }

        public string UserAgent { get; set; }

        public int Timeout { get; set; }

        public bool UseSynchronizationContext { get; set; }

        public IAuthenticator Authenticator { get; set; }

        public virtual string BaseUrl
        {
            get
            {
                return this._baseUrl;
            }
            set
            {
                this._baseUrl = value;
                if (this._baseUrl == null || !this._baseUrl.EndsWith("/"))
                    return;
                this._baseUrl = this._baseUrl.Substring(0, this._baseUrl.Length - 1);
            }
        }

        static DebugRestClient()
        {
        }

        public DebugRestClient()
        {
            this.ContentHandlers = (IDictionary<string, IDeserializer>)new Dictionary<string, IDeserializer>();
            this.AcceptTypes = (IList<string>)new List<string>();
            this.DefaultParameters = (IList<Parameter>)new List<Parameter>();
            this.AddHandler("application/json", (IDeserializer)new JsonDeserializer());
            this.AddHandler("application/xml", (IDeserializer)new XmlDeserializer());
            this.AddHandler("text/json", (IDeserializer)new JsonDeserializer());
            this.AddHandler("text/x-json", (IDeserializer)new JsonDeserializer());
            this.AddHandler("text/javascript", (IDeserializer)new JsonDeserializer());
            this.AddHandler("text/xml", (IDeserializer)new XmlDeserializer());
            this.AddHandler("*", (IDeserializer)new XmlDeserializer());
            this.FollowRedirects = true;
        }

        public DebugRestClient(string baseUrl)
            : this()
        {
            this.BaseUrl = baseUrl;
        }

        public void AddHandler(string contentType, IDeserializer deserializer)
        {
            this.ContentHandlers[contentType] = deserializer;
            if (!(contentType != "*"))
                return;
            this.AcceptTypes.Add(contentType);
            string str = string.Join(", ", Enumerable.ToArray<string>((IEnumerable<string>)this.AcceptTypes));
            RestClientExtensions.RemoveDefaultParameter((IRestClient)this, "Accept");
            RestClientExtensions.AddDefaultParameter((IRestClient)this, "Accept", (object)str, ParameterType.HttpHeader);
        }

        public void RemoveHandler(string contentType)
        {
            this.ContentHandlers.Remove(contentType);
            this.AcceptTypes.Remove(contentType);
            RestClientExtensions.RemoveDefaultParameter((IRestClient)this, "Accept");
        }

        public void ClearHandlers()
        {
            this.ContentHandlers.Clear();
            this.AcceptTypes.Clear();
            RestClientExtensions.RemoveDefaultParameter((IRestClient)this, "Accept");
        }

        private IDeserializer GetHandler(string contentType)
        {
            if (string.IsNullOrEmpty(contentType) && this.ContentHandlers.ContainsKey("*"))
                return this.ContentHandlers["*"];
            int length = contentType.IndexOf(';');
            if (length > -1)
                contentType = contentType.Substring(0, length);
            IDeserializer deserializer = (IDeserializer)null;
            if (this.ContentHandlers.ContainsKey(contentType))
                deserializer = this.ContentHandlers[contentType];
            else if (this.ContentHandlers.ContainsKey("*"))
                deserializer = this.ContentHandlers["*"];
            return deserializer;
        }

        private void AuthenticateIfNeeded(DebugRestClient client, IRestRequest request)
        {
            if (this.Authenticator == null)
                return;
            this.Authenticator.Authenticate((IRestClient)client, request);
        }

        public Uri BuildUri(IRestRequest request)
        {
            string uriString = request.Resource;
            foreach (Parameter parameter in Enumerable.Where<Parameter>((IEnumerable<Parameter>)request.Parameters, (Func<Parameter, bool>)(p => p.Type == ParameterType.UrlSegment)))
                uriString = uriString.Replace("{" + parameter.Name + "}", global::RestSharp.Extensions.StringExtensions.UrlEncode(parameter.Value.ToString()));
            if (!string.IsNullOrEmpty(uriString) && uriString.StartsWith("/"))
                uriString = uriString.Substring(1);
            if (!string.IsNullOrEmpty(this.BaseUrl))
                uriString = !string.IsNullOrEmpty(uriString) ? string.Format("{0}/{1}", (object)this.BaseUrl, (object)uriString) : this.BaseUrl;
            if (request.Method != Method.POST && request.Method != Method.PUT && request.Method != Method.PATCH && Enumerable.Any<Parameter>((IEnumerable<Parameter>)request.Parameters, (Func<Parameter, bool>)(p => p.Type == ParameterType.GetOrPost)))
            {
                if (uriString.EndsWith("/"))
                    uriString = uriString.Substring(0, uriString.Length - 1);
                string str = this.EncodeParameters(request);
                uriString = string.Format("{0}?{1}", (object)uriString, (object)str);
            }
            return new Uri(uriString);
        }

        private string EncodeParameters(IRestRequest request)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Parameter parameter in Enumerable.Where<Parameter>((IEnumerable<Parameter>)request.Parameters, (Func<Parameter, bool>)(p => p.Type == ParameterType.GetOrPost)))
            {
                if (stringBuilder.Length > 1)
                    stringBuilder.Append("&");
                stringBuilder.AppendFormat("{0}={1}", (object)global::RestSharp.Extensions.StringExtensions.UrlEncode(parameter.Name), (object)global::RestSharp.Extensions.StringExtensions.UrlEncode(parameter.Value.ToString()));
            }
            return ((object)stringBuilder).ToString();
        }

        private void ConfigureHttp(IRestRequest request, IHttp http)
        {
            http.CookieContainer = this.CookieContainer;
            http.ResponseWriter = request.ResponseWriter;
            foreach (Parameter parameter in (IEnumerable<Parameter>)this.DefaultParameters)
            {
                Parameter p = parameter;
                if (!Enumerable.Any<Parameter>((IEnumerable<Parameter>)request.Parameters, (Func<Parameter, bool>)(p2 =>
                {
                    if (p2.Name == p.Name)
                        return p2.Type == p.Type;
                    else
                        return false;
                })))
                    request.AddParameter(p);
            }
            if (!Enumerable.Any<Parameter>((IEnumerable<Parameter>)request.Parameters, (Func<Parameter, bool>)(p2 => p2.Name.ToLowerInvariant() == "accept")))
            {
                string str = string.Join(", ", Enumerable.ToArray<string>((IEnumerable<string>)this.AcceptTypes));
                request.AddParameter("Accept", (object)str, ParameterType.HttpHeader);
            }
            http.Url = this.BuildUri(request);
            string input = this.UserAgent ?? http.UserAgent;
            http.UserAgent = global::RestSharp.Extensions.StringExtensions.HasValue(input) ? input : "RestSharp " + ((object)DebugRestClient.version).ToString();
            int num = request.Timeout > 0 ? request.Timeout : this.Timeout;
            if (num > 0)
                http.Timeout = num;
            http.FollowRedirects = this.FollowRedirects;
            if (this.ClientCertificates != null)
                http.ClientCertificates = this.ClientCertificates;
            http.MaxRedirects = this.MaxRedirects;
            if (request.Credentials != null)
                http.Credentials = request.Credentials;
            foreach (HttpHeader httpHeader in Enumerable.Select<Parameter, HttpHeader>(Enumerable.Where<Parameter>((IEnumerable<Parameter>)request.Parameters, (Func<Parameter, bool>)(p => p.Type == ParameterType.HttpHeader)), (Func<Parameter, HttpHeader>)(p => new HttpHeader()
            {
                Name = p.Name,
                Value = p.Value.ToString()
            })))
                http.Headers.Add(httpHeader);
            foreach (HttpCookie httpCookie in Enumerable.Select<Parameter, HttpCookie>(Enumerable.Where<Parameter>((IEnumerable<Parameter>)request.Parameters, (Func<Parameter, bool>)(p => p.Type == ParameterType.Cookie)), (Func<Parameter, HttpCookie>)(p => new HttpCookie()
            {
                Name = p.Name,
                Value = p.Value.ToString()
            })))
                http.Cookies.Add(httpCookie);
            foreach (HttpParameter httpParameter in Enumerable.Select<Parameter, HttpParameter>(Enumerable.Where<Parameter>((IEnumerable<Parameter>)request.Parameters, (Func<Parameter, bool>)(p =>
            {
                if (p.Type == ParameterType.GetOrPost)
                    return p.Value != null;
                else
                    return false;
            })), (Func<Parameter, HttpParameter>)(p => new HttpParameter()
            {
                Name = p.Name,
                Value = p.Value.ToString()
            })))
                http.Parameters.Add(httpParameter);
            foreach (FileParameter fileParameter in request.Files)
                http.Files.Add(new HttpFile()
                {
                    Name = fileParameter.Name,
                    ContentType = fileParameter.ContentType,
                    Writer = fileParameter.Writer,
                    FileName = fileParameter.FileName,
                    ContentLength = fileParameter.ContentLength
                });
            Parameter parameter1 = Enumerable.FirstOrDefault<Parameter>(Enumerable.Where<Parameter>((IEnumerable<Parameter>)request.Parameters, (Func<Parameter, bool>)(p => p.Type == ParameterType.RequestBody)));
            if (parameter1 != null)
            {
                object obj = parameter1.Value;
                if (obj is byte[])
                    http.RequestBodyBytes = (byte[])obj;
                else
                    http.RequestBody = parameter1.Value.ToString();
                http.RequestContentType = parameter1.Name;
            }
            this.ConfigureProxy(http);
        }

        private void ConfigureProxy(IHttp http)
        {
            if (this.Proxy == null)
                return;
            http.Proxy = this.Proxy;
        }

        private RestResponse ConvertToRestResponse(IRestRequest request, HttpResponse httpResponse)
        {
            RestResponse restResponse = new RestResponse();
            restResponse.Content = httpResponse.Content;
            restResponse.ContentEncoding = httpResponse.ContentEncoding;
            restResponse.ContentLength = httpResponse.ContentLength;
            restResponse.ContentType = httpResponse.ContentType;
            restResponse.ErrorException = httpResponse.ErrorException;
            restResponse.ErrorMessage = httpResponse.ErrorMessage;
            restResponse.RawBytes = httpResponse.RawBytes;
            restResponse.ResponseStatus = httpResponse.ResponseStatus;
            restResponse.ResponseUri = httpResponse.ResponseUri;
            restResponse.Server = httpResponse.Server;
            restResponse.StatusCode = httpResponse.StatusCode;
            restResponse.StatusDescription = httpResponse.StatusDescription;
            restResponse.Request = request;
            foreach (HttpHeader httpHeader in (IEnumerable<HttpHeader>)httpResponse.Headers)
                restResponse.Headers.Add(new Parameter()
                {
                    Name = httpHeader.Name,
                    Value = (object)httpHeader.Value,
                    Type = ParameterType.HttpHeader
                });
            foreach (HttpCookie httpCookie in (IEnumerable<HttpCookie>)httpResponse.Cookies)
                restResponse.Cookies.Add(new RestResponseCookie()
                {
                    Comment = httpCookie.Comment,
                    CommentUri = httpCookie.CommentUri,
                    Discard = httpCookie.Discard,
                    Domain = httpCookie.Domain,
                    Expired = httpCookie.Expired,
                    Expires = httpCookie.Expires,
                    HttpOnly = httpCookie.HttpOnly,
                    Name = httpCookie.Name,
                    Path = httpCookie.Path,
                    Port = httpCookie.Port,
                    Secure = httpCookie.Secure,
                    TimeStamp = httpCookie.TimeStamp,
                    Value = httpCookie.Value,
                    Version = httpCookie.Version
                });
            return restResponse;
        }

        private IRestResponse<T> Deserialize<T>(IRestRequest request, IRestResponse raw)
        {
            request.OnBeforeDeserialization(raw);
            IDeserializer handler = this.GetHandler(raw.ContentType);
            handler.RootElement = request.RootElement;
            handler.DateFormat = request.DateFormat;
            handler.Namespace = request.XmlNamespace;
            IRestResponse<T> restResponse = (IRestResponse<T>)new RestResponse<T>();
            try
            {
                restResponse = ResponseExtensions.toAsyncResponse<T>(raw);
                restResponse.Data = handler.Deserialize<T>(raw);
                restResponse.Request = request;
            }
            catch (Exception ex)
            {
                restResponse.ResponseStatus = ResponseStatus.Error;
                restResponse.ErrorMessage = ex.Message;
                restResponse.ErrorException = ex;
            }
            return restResponse;
        }

        public virtual RestRequestAsyncHandle ExecuteAsync(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            string name = System.Enum.GetName(typeof(Method), (object)request.Method);
            switch (request.Method)
            {
                case Method.POST:
                case Method.PUT:
                case Method.PATCH:
                    return this.ExecuteAsync(request, callback, name, new Func<IHttp, Action<HttpResponse>, string, HttpWebRequest>(DebugRestClient.DoAsPostAsync));
                default:
                    return this.ExecuteAsync(request, callback, name, new Func<IHttp, Action<HttpResponse>, string, HttpWebRequest>(DebugRestClient.DoAsGetAsync));
            }
        }

        public virtual RestRequestAsyncHandle ExecuteAsyncGet(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, string httpMethod)
        {
            return this.ExecuteAsync(request, callback, httpMethod, new Func<IHttp, Action<HttpResponse>, string, HttpWebRequest>(DebugRestClient.DoAsGetAsync));
        }

        public virtual RestRequestAsyncHandle ExecuteAsyncPost(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, string httpMethod)
        {
            request.Method = Method.POST;
            return this.ExecuteAsync(request, callback, httpMethod, new Func<IHttp, Action<HttpResponse>, string, HttpWebRequest>(DebugRestClient.DoAsPostAsync));
        }

        private RestRequestAsyncHandle ExecuteAsync(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, string httpMethod, Func<IHttp, Action<HttpResponse>, string, HttpWebRequest> getWebRequest)
        {
            IHttp http = this.HttpFactory.Create();
            this.AuthenticateIfNeeded(this, request);
            this.ConfigureHttp(request, http);
            RestRequestAsyncHandle asyncHandle = new RestRequestAsyncHandle();
            Action<HttpResponse> action = (Action<HttpResponse>)(r => this.ProcessResponse(request, r, asyncHandle, callback));
            if (this.UseSynchronizationContext && SynchronizationContext.Current != null)
            {
                SynchronizationContext ctx = SynchronizationContext.Current;
                Action<HttpResponse> cb = action;
                action = (Action<HttpResponse>)(resp => ctx.Post((SendOrPostCallback)(s => cb(resp)), (object)null));
            }
            asyncHandle.WebRequest = getWebRequest(http, action, httpMethod);
            return asyncHandle;
        }

        private static HttpWebRequest DoAsGetAsync(IHttp http, Action<HttpResponse> response_cb, string method)
        {
            return http.AsGetAsync(response_cb, method);
        }

        private static HttpWebRequest DoAsPostAsync(IHttp http, Action<HttpResponse> response_cb, string method)
        {
            return http.AsPostAsync(response_cb, method);
        }

        private void ProcessResponse(IRestRequest request, HttpResponse httpResponse, RestRequestAsyncHandle asyncHandle, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            RestResponse restResponse = this.ConvertToRestResponse(request, httpResponse);
            callback((IRestResponse)restResponse, asyncHandle);
        }

        public virtual RestRequestAsyncHandle ExecuteAsync<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback)
        {
            return this.ExecuteAsync(request, (Action<IRestResponse, RestRequestAsyncHandle>)((response, asyncHandle) => this.DeserializeResponse<T>(request, callback, response, asyncHandle)));
        }

        public virtual RestRequestAsyncHandle ExecuteAsyncGet<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback, string httpMethod)
        {
            return this.ExecuteAsyncGet(request, (Action<IRestResponse, RestRequestAsyncHandle>)((response, asyncHandle) => this.DeserializeResponse<T>(request, callback, response, asyncHandle)), httpMethod);
        }

        public virtual RestRequestAsyncHandle ExecuteAsyncPost<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback, string httpMethod)
        {
            return this.ExecuteAsyncPost(request, (Action<IRestResponse, RestRequestAsyncHandle>)((response, asyncHandle) => this.DeserializeResponse<T>(request, callback, response, asyncHandle)), httpMethod);
        }

        private void DeserializeResponse<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback, IRestResponse response, RestRequestAsyncHandle asyncHandle)
        {
            IRestResponse<T> restResponse = (IRestResponse<T>)(response as RestResponse<T>);
            if (response.ResponseStatus != ResponseStatus.Aborted)
                restResponse = this.Deserialize<T>(request, response);
            callback(restResponse, asyncHandle);
        }

        public byte[] DownloadData(IRestRequest request)
        {
            return this.Execute(request).RawBytes;
        }

        public virtual IRestResponse Execute(IRestRequest request)
        {
            string name = System.Enum.GetName(typeof(Method), (object)request.Method);
            switch (request.Method)
            {
                case Method.POST:
                case Method.PUT:
                case Method.PATCH:
                    return this.Execute(request, name, new Func<IHttp, string, HttpResponse>(DebugRestClient.DoExecuteAsPost));
                default:
                    return this.Execute(request, name, new Func<IHttp, string, HttpResponse>(DebugRestClient.DoExecuteAsGet));
            }
        }

        public IRestResponse ExecuteAsGet(IRestRequest request, string httpMethod)
        {
            return this.Execute(request, httpMethod, new Func<IHttp, string, HttpResponse>(DebugRestClient.DoExecuteAsGet));
        }

        public IRestResponse ExecuteAsPost(IRestRequest request, string httpMethod)
        {
            request.Method = Method.POST;
            return this.Execute(request, httpMethod, new Func<IHttp, string, HttpResponse>(DebugRestClient.DoExecuteAsPost));
        }

        private IRestResponse Execute(IRestRequest request, string httpMethod, Func<IHttp, string, HttpResponse> getResponse)
        {
            this.AuthenticateIfNeeded(this, request);
            IRestResponse restResponse = (IRestResponse)new RestResponse();
            try
            {
                IHttp http = this.HttpFactory.Create();
                this.ConfigureHttp(request, http);
                restResponse = (IRestResponse)this.ConvertToRestResponse(request, getResponse(http, httpMethod));
                restResponse.Request = request;
                restResponse.Request.IncreaseNumAttempts();
            }
            catch (Exception ex)
            {
                restResponse.ResponseStatus = ResponseStatus.Error;
                restResponse.ErrorMessage = ex.Message;
                restResponse.ErrorException = ex;
            }
            return restResponse;
        }

        private static HttpResponse DoExecuteAsGet(IHttp http, string method)
        {
            return http.AsGet(method);
        }

        private static HttpResponse DoExecuteAsPost(IHttp http, string method)
        {
            return http.AsPost(method);
        }

        public virtual IRestResponse<T> Execute<T>(IRestRequest request) where T : new()
        {
            return this.Deserialize<T>(request, this.Execute(request));
        }

        public IRestResponse<T> ExecuteAsGet<T>(IRestRequest request, string httpMethod) where T : new()
        {
            return this.Deserialize<T>(request, this.ExecuteAsGet(request, httpMethod));
        }

        public IRestResponse<T> ExecuteAsPost<T>(IRestRequest request, string httpMethod) where T : new()
        {
            return this.Deserialize<T>(request, this.ExecuteAsPost(request, httpMethod));
        }
    }
}
