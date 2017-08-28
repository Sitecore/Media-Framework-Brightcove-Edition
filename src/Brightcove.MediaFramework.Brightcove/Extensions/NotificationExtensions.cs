using System;
using System.Text;
using Brightcove.MediaFramework.Brightcove.Entities;

namespace Brightcove.MediaFramework.Brightcove.Extensions
{
    public static class NotificationExtensions
    {
        private const string Title = "TITLE";

        private const string Asset = "ASSET";

        private const string Success = "SUCCESS";

        private const string Create = "CREATE";

        public static bool IsVideo(this Notification notification)
        {
            return Title.Equals(notification.EntityType, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsAsset(this Notification notification)
        {
            return Asset.Equals(notification.EntityType, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsSuccess(this Notification notification)
        {
            return Success.Equals(notification.Status, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsCreate(this Notification notification)
        {
            return Create.Equals(notification.Action, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsVideoUpload(this Notification notification)
        {
            return !string.IsNullOrEmpty(notification.Entity)
                   && notification.IsVideo()
                   && notification.IsCreate();
        }

        public static bool IsAssetUpload(this Notification notification)
        {
            return !string.IsNullOrEmpty(notification.Entity)
                   && notification.IsAsset()
                   && notification.IsCreate();
        }
    }
}
