using System.Text;
using Sitecore.Data;

namespace AgencyOasis.MediaFramework.Brightcove.Extensions
{
    public static class IDExtensions
    {
        public static string ToUrlString(this ID value)
        {
            return value.ToString().Replace("{", "").Replace("}", "").ToLower();
        }
    }
}
