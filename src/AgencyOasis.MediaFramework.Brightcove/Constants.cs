namespace AgencyOasis.MediaFramework.Brightcove
{
    public static class Constants
    {
        public static readonly string BrightCoveOauthService = "mediaframework_brightcove_oauth";
        public static readonly string BrightCoveCmsService = "mediaframework_brightcove_cms";
        public static readonly string BrightCoveDynamicIngestService = "mediaframework_brightcove_dynamic_ingest";

        public static readonly string BrightcoveVideoCustomFieldsRouteName = "Sitecore.MediaFramework.Brightcove.Video.Customfields";
        public static readonly string BrightcoveVideoTextTracksRouteName = "Sitecore.MediaFramework.Brightcove.Video.TextTracks";
        public static readonly string BrightcoveFileDownloadRouteName = "Sitecore.MediaFramework.Brightcove.File.Download";

        public static readonly string MvcDefaultRouteName = "Sitecore.MediaFramework.Brightcove.DefaultMvc";
        public static readonly string WebApiDefaultRouteName = "Sitecore.MediaFramework.Brightcove.DefaultApi";
        public static readonly string BrightcoveIngestionCallbackRouteName = "Sitecore.MediaFramework.Brightcove.Ingestion.Callback";

        public static readonly string BrightcoveFileDownloadRouteTemplate = "/files/{fileId}";

        public static readonly string BrightcoveVideoCustomFieldsRouteTemplate = "/accounts/{accountItemId}/videos/{videoId}/customfields";
        public static readonly string BrightcoveVideoTextTracksRouteTemplate = "/accounts/{accountItemId}/videos/{videoId}/texttracks";

        public static readonly string BrightcoveIngestionCallbackRouteTemplate = "/ingestioncallback/{operationId}";

        public static readonly string DefaultRouteTemplate = "/{controller}/{action}";
    }
}
