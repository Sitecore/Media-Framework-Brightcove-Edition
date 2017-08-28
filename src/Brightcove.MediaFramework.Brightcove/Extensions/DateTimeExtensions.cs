using System;

namespace Brightcove.MediaFramework.Brightcove.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToFolderName(this DateTime value)
        {
            return value.ToString("yyyyMMddhhmmssfffffff");
        }
    }
}