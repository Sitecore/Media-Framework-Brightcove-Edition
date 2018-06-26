using System.Collections.Generic;

namespace Brightcove.MediaFramework.Brightcove.Entities.ViewModels
{
    public class VideoTextTracks
    {
        public VideoTextTracks()
        {
            DefaultMimeType = "text/vtt";
        }

        public Video Video { get;set; }
        
        public IList<TextTrackInfo> Tracks { get; set; }

        public IList<string> Languages { get; set; }

        public IList<string> KindList { get; set; }

        public string DefaultMimeType { get; set; }

        public bool UploadingTracks { get; set; }
    }
}