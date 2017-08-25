using System.Collections.Generic;
using System.Web;

namespace Brightcove.MediaFramework.Brightcove.Entities.ViewModels
{
    public class TextTrackInfo : VideoTextTrack
    {
        public bool IsDelete { get; set; }
        
        public bool IsUpload { get; set; }

        public HttpPostedFileBase File { get; set; }
    }
}