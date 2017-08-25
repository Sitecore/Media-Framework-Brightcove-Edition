using Brightcove.MediaFramework.Brightcove.Upload;
using Sitecore.MediaFramework.Diagnostics;
using System.Web.Mvc;
using Brightcove.MediaFramework.Brightcove.Configuration;

namespace Brightcove.MediaFramework.Brightcove.Controllers
{
    public class FilesController : Controller
    {
        [HttpGet]
        public ActionResult Get(string fileId)
        {
            LogHelper.Info("brightcove Files controller - Retrieve File for id " + fileId, this);

            var info = StorageServiceManager.RetrieveFile(fileId);

            if (info != null)
            {
                if (Settings.BrightcoveTextTracksMimeTypes.Contains(info.MimeType))
                {
                    return new TextResult
                    {
                        ContentType = info.MimeType,
                        FileName = info.Name,
                        Data = System.Text.Encoding.UTF8.GetString(info.Bytes)
                    };
                }
                return new BinaryResult
                {
                    ContentType = info.MimeType,
                    FileName = info.Name,
                    Data = info.Bytes
                };
            }

            return new HttpNotFoundResult();
        }
    }
}