using System.Web.Http;
using System.Web.Routing;
using Brightcove.MediaFramework.Brightcove.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;
using MVCRouteCollectionExtensions = System.Web.Mvc.RouteCollectionExtensions;

namespace Brightcove.MediaFramework.Brightcove.Pipelines.Initialize
{
    public class RegisterRoutes
    {
        public virtual void Process(PipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            RegisterMvcRoutes(RouteTable.Routes, args);
            RegisterApiRoutes(RouteTable.Routes, args);
        }

        protected virtual void RegisterMvcRoutes(RouteCollection routes, PipelineArgs args)
        {
            //// download file
            MVCRouteCollectionExtensions.MapRoute(
                routes,
                Constants.BrightcoveFileDownloadRouteName,
                Settings.FileDownloadRouteTemplate,
                new { fileId = string.Empty, controller = "Files", action = "Get" });

            //// manage video custom fields
            MVCRouteCollectionExtensions.MapRoute(
                routes,
                Constants.BrightcoveVideoCustomFieldsRouteName,
                Settings.CustomFieldsRouteTemplate,
                new { accountItemId = string.Empty, videoId = string.Empty, controller = "VideoCustomFields", action = "CustomFields" });

            //// manage video text tracks
            MVCRouteCollectionExtensions.MapRoute(
                routes,
                Constants.BrightcoveVideoTextTracksRouteName,
                Settings.TextTracksRouteTemplate,
                new { accountItemId = string.Empty, videoId = string.Empty, controller = "VideoTextTracks", action = "TextTracks" });

            //// manage ingestion callback
            MVCRouteCollectionExtensions.MapRoute(
                routes,
                Constants.BrightcoveIngestionCallbackRouteName,
                Settings.IngestionCallbackRouteTemplate,
                new { accountId = string.Empty, operationId = string.Empty, controller = "IngestionCallback", action = "Post" });
        }

        protected virtual void RegisterApiRoutes(RouteCollection routes, PipelineArgs args)
        {
            //////// manage ingestion callback
            ////routes.MapHttpRoute(
            ////    Constants.BrightcoveIngestionCallbackRouteName, 
            ////    Settings.IngestionCallbackRouteTemplate,
            ////    new { accountId = string.Empty, operationId = string.Empty, controller = "IngestionCallback", action = "Post" });
        }

        protected virtual void RegisterDefaultRoutes(RouteCollection routes, PipelineArgs args)
        {
            //// default mvc route
            ////MVCRouteCollectionExtensions.MapRoute(
            ////    routes,
            ////    Constants.MvcDefaultRouteName,
            ////    Settings.DefaultRouteTemplate);

            routes.MapHttpRoute(Constants.WebApiDefaultRouteName, Settings.DefaultRouteTemplate);
        }
    }
}
