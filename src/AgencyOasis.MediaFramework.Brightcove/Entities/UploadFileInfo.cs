using System.IO;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.IO;
using Sitecore.Resources.Media;

namespace Brightcove.MediaFramework.Brightcove.Entities
{
    public class UploadFileInfo
    {
        private string _extension;

        private string _mimeType;

        public string Name { get; set; }

        public byte[] Bytes { get; set; }

        public string Id { get; set; }

        public string MediaId { get; set; }

        public string Extension
        {
            get
            {
                if (string.IsNullOrEmpty(_extension))
                {
                    _extension = FileUtil.GetExtension(Name);
                }
                return _extension;
            }
        }

        public string MimeType
        {
            get
            {
                if (string.IsNullOrEmpty(_mimeType))
                {
                    _mimeType = MediaManager.Config.GetMimeType(Extension);
                }
                return _mimeType;
            }
            set { _mimeType = value; }
        }

        public int Size
        {
            get { return Bytes.Length; }
        }

        public string FileNameWithoutExtension
        {
            get { return Path.GetFileNameWithoutExtension(Name); }
        }

        public string Url { get; set; }

    }
}