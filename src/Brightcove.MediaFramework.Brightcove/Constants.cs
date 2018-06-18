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
    public static readonly string BrightcovePlaylistTemplateId = "{0E24292F-D7A5-4BA2-BCA0-CD5F14A89634}";

    public static readonly string SitecoreRestSharpService = "mediaframework_brightcove";
    public static readonly string IndexName = "mediaframework_brightcove_index";

    public static readonly string EmbedJavascript = "Javascript";
    public static readonly string EmbedIframe = "iFrame";
    public static readonly string SizingResponsive = "Responsive";
    public static readonly string SizingFixed = "Fixed";
  }

  public static class BrightcovePlayerParameters
  {
    public static readonly string ItemId = "itemId";
    public static readonly string Template = "template";
    public static readonly string PlayerId = "ID";
    public static readonly string Source = "source";
    public static readonly string MediaId = "ID";
    public static readonly string Width = "width";
    public static readonly string Height = "height";
    public static readonly string Autoplay = "autoplay";
    public static readonly string Muted = "muted";
    public static readonly string EmbedStyle = "embed";
    public static readonly string Sizing = "sizing";
    public static readonly string AspectRatio = "aspectRatio";
    public static readonly string Ratio16X9 = "16:9";
    public static readonly string Ratio4X3 = "4:3";
    public static readonly string RatioCustom = "Custom";
    public static readonly string PublisherId = "PublisherID";
    public static readonly string PlaylistId = "ID";
    public static readonly string PlaylistCreated = "CreationDate";
    public static readonly string PlaylistUpdated = "LastModifiedDate";
    public static readonly string PlaylistType = "PlaylistType";
    public static readonly string AccountName = "AccountName";
    public static readonly string ForceRender = "forceRender";
  }
}
