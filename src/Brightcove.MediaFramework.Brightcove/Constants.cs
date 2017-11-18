namespace Brightcove.MediaFramework.Brightcove
{
    public static class Constants
    {
        public static readonly string BrightCoveOauthService = "mediaframework_brightcove_oauth";
        public static readonly string BrightCoveCmsService = "mediaframework_brightcove_cms";
        public static readonly string BrightCoveDynamicIngestService = "mediaframework_brightcove_dynamic_ingest";

        public static readonly string BrightcoveVideoCustomFieldsRouteName = "Video.Customfields";
        public static readonly string BrightcoveVideoTextTracksRouteName = "Video.TextTracks";
        public static readonly string BrightcoveFileDownloadRouteName = "File.Download";

        public static readonly string MvcDefaultRouteName = "DefaultMvc";
        public static readonly string WebApiDefaultRouteName = "DefaultApi";
        public static readonly string BrightcoveIngestionCallbackRouteName = "Ingestion.Callback";

        public static readonly string BrightcoveFileDownloadRouteTemplate = "/files/{fileId}";

        public static readonly string BrightcoveVideoCustomFieldsRouteTemplate = "/accounts/{accountItemId}/videos/{videoId}/customfields";
        public static readonly string BrightcoveVideoTextTracksRouteTemplate = "/accounts/{accountItemId}/videos/{videoId}/texttracks";

        public static readonly string BrightcoveIngestionCallbackRouteTemplate = "/ingestioncallback/{operationId}";

        public static readonly string DefaultRouteTemplate = "/{controller}/{action}";

        public static readonly string SitecoreRestSharpService = "mediaframework_brightcove";
        public static readonly string IndexName = "mediaframework_brightcove_index";
    }
}
