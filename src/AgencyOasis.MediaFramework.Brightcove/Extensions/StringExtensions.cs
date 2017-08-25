using System.Text;

namespace Brightcove.MediaFramework.Brightcove.Extensions
{
    public static class StringExtensions
    {
        public static string ToBase64String(this string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            return System.Convert.ToBase64String(bytes);
        }
    }
}
