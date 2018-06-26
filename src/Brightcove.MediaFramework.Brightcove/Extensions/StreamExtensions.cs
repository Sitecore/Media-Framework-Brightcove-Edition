using System.IO;
using System.Text;

namespace Brightcove.MediaFramework.Brightcove.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] ToBytes(this Stream stream, long size)
        {
            var buffer = new byte[size];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
