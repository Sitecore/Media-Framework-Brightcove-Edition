using System.Text;
using Sitecore.Data;

namespace Brightcove.MediaFramework.Brightcove.Extensions
{
    public static class IDExtensions
    {
        public static string ToUrlString(this ID value)
        {
            return value.ToString().Replace("{", "").Replace("}", "").ToLower();
        }
    }
}
