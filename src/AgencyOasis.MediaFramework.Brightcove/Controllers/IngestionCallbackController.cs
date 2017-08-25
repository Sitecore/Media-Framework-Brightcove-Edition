using Brightcove.MediaFramework.Brightcove.Entities;
using Brightcove.MediaFramework.Brightcove.Pipelines.HandleCallback;
using Newtonsoft.Json;
using Sitecore.MediaFramework.Diagnostics;
using Sitecore.Services.Infrastructure.Web.Http;
using System;
using System.Web.Http;
using System.Web.Mvc;

namespace Brightcove.MediaFramework.Brightcove.Controllers
{
    public class IngestionCallbackController : Controller
    {
        [System.Web.Http.HttpPost]
        public ActionResult Post(Notification notification, string operationId)
        {
            if (notification == null || string.IsNullOrEmpty(operationId))
            {
                return HttpNotFound();
            }

            try
            {
                LogHelper.Info("brightcove ingestion callback controller for operation id = " + operationId + " and notification = " + JsonConvert.SerializeObject(notification), this);

                var args = new HandleCallbackArgs
                {
                    Notification = notification,
                    OperationId = operationId
                };
                
                Sitecore.Pipelines.CorePipeline.Run("Brightcove.HandleCallback", args);
            }
            catch (Exception ex)
            {
                LogHelper.Error("brightcove video upload notification -", this, ex);
                throw;
            }

            return this.Json(JsonConvert.SerializeObject(notification), JsonRequestBehavior.AllowGet);
        }
    }
}