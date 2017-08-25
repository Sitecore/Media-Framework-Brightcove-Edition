using RestSharp;

namespace Brightcove.MediaFramework.Brightcove.Extensions
{
    public static class RestRequestExtensions
    {
        public static IRestRequest AddOffset(this IRestRequest request, int offset)
        {
            request.AddParameter(new Parameter()
            {
                Name = "offset",
                Type = ParameterType.UrlSegment,
                Value = offset
            });

            return request;
        }

        public static IRestRequest AddAccountId(this IRestRequest request, string id)
        {
            request.AddParameter(new Parameter()
            {
                Name = "account_id",
                Type = ParameterType.UrlSegment,
                Value = id
            });

            return request;
        }

        public static IRestRequest AddAuthorization(this IRestRequest request, string value)
        {
            request.AddParameter(new Parameter()
            {
                Name = "Authorization",
                Type = ParameterType.HttpHeader,
                Value = value
            });

            return request;
        }

        public static IRestRequest AddLimit(this IRestRequest request, int limit)
        {
            request.AddParameter(new Parameter()
            {
                Name = "limit",
                Type = ParameterType.UrlSegment,
                Value = limit
            });

            return request;
        }

        public static IRestRequest AddId(this IRestRequest request, string name, string value)
        {
            request.Parameters.Add(new Parameter()
            {
                Name = name,
                Type = ParameterType.UrlSegment,
                Value = value
            });

            return request;
        }

        public static IRestRequest AddVideoId(this IRestRequest request, string value)
        {
            request.Parameters.Add(new Parameter()
            {
                Name = "video_id",
                Type = ParameterType.UrlSegment,
                Value = value
            });

            return request;
        }
    }
}
