using Brightcove.MediaFramework.Brightcove.Entities;
using Brightcove.MediaFramework.Brightcove.Entities.ViewModels;
using Brightcove.MediaFramework.Brightcove.Proxy.CMS;
using Brightcove.MediaFramework.Brightcove.Security;
using Sitecore.Data;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Brightcove.MediaFramework.Brightcove.Controllers
{
    public class VideoCustomFieldsController : Controller
    {
        [HttpGet]
        public ActionResult CustomFields(string accountItemId, string videoId)
        {
            var accountItem = Sitecore.Context.Database.GetItem(new ID(accountItemId));
            var authenticator = new BrightcoveAuthenticator(accountItem);
            var proxy = new VideoProxy(authenticator);
            var customFieldsResponse = proxy.RetrieveCustomFields();
            var video = proxy.RetrieveById(videoId);
            var fields = (from cf in customFieldsResponse.CustomFields
                          from vf in video.CustomFields.Select(i => new FieldInfo { Id = i.Key, Value = i.Value })
                          where cf.Id.Equals(vf.Id, StringComparison.OrdinalIgnoreCase)
                          select MapValue(vf, cf)).ToList();

            fields = customFieldsResponse.CustomFields.Where(i => i.EnumValues == null || i.EnumValues.Count > 0).Select(AddEmptyEnumValue).ToList();
            var model = new VideoCustomFields
            {
                Video = video,
                Fields = fields
            };

            ViewData["AjaxType"] = "get";
            return View("~/sitecore modules/Web/MediaFramework/MVC/Views/Brightcove/VideoCustomFields.cshtml", model);
        }

        [HttpPost]
        public ActionResult CustomFields(string accountItemId, string videoId, VideoCustomFields videoCustomFields)
        {
            var accountItem = Sitecore.Context.Database.GetItem(new ID(accountItemId));
            var authenticator = new BrightcoveAuthenticator(accountItem);
            var proxy = new VideoProxy(authenticator);

            var customFieldsResponse = proxy.RetrieveCustomFields();

            proxy.UpdateCustomFields(videoId, videoCustomFields.Fields);

            var video = proxy.RetrieveById(videoId);

            var fields = (from cf in customFieldsResponse.CustomFields
                          from vf in video.CustomFields.Select(i => new FieldInfo { Id = i.Key, Value = i.Value })
                          where cf.Id.Equals(vf.Id, StringComparison.OrdinalIgnoreCase)
                          select MapValue(vf, cf)).ToList();

            fields = customFieldsResponse.CustomFields.Where(i => i.EnumValues == null || i.EnumValues.Count > 0).Select(AddEmptyEnumValue).ToList();

            var model = new VideoCustomFields
            {
                Video = video,
                Fields = fields
            };

            ViewData["AjaxType"] = "post";
            return View("~/sitecore modules/Web/MediaFramework/MVC/Views/Brightcove/VideoCustomFields.cshtml", model);
        }

        private static FieldInfo MapValue(FieldInfo source, FieldInfo target)
        {
            target.Value = source.Value;
            return target;
        }

        private static FieldInfo AddEmptyEnumValue(FieldInfo fieldInfo)
        {
            if (fieldInfo.EnumValues != null && fieldInfo.EnumValues.Count > 0)
            {
                fieldInfo.EnumValues.Insert(0, "");
                fieldInfo.EnumValues.Sort((s1, s2) => String.Compare(s1, s2, StringComparison.CurrentCulture));
            }

            return fieldInfo;
        }
    }
}