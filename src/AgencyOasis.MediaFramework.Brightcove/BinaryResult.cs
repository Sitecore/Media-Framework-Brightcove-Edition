using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Brightcove.MediaFramework.Brightcove
{
    public class BinaryResult : ActionResult
    {
        public byte[] Data { get; set; }
        public bool IsAttachment { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ContentType = ContentType;
            if (!string.IsNullOrEmpty(FileName))
            {
                context.HttpContext.Response.AddHeader("content-disposition",
                    ((IsAttachment) ? "attachment;filename=" : "inline;filename=") +
                    FileName);
            }
            context.HttpContext.Response.BinaryWrite(Data);
        }
    }
}
