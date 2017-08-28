using Sitecore.Analytics;
using Sitecore.Analytics.Data;
using Sitecore.Analytics.Tracking;
using Sitecore.Configuration;
using Sitecore.SecurityModel.License;

namespace Brightcove.MediaFramework.Brightcove.Analytics
{
    public abstract class EventTrigger : Sitecore.MediaFramework.Analytics.EventTrigger
    {
        protected override void TriggerEvent(PageEventData eventData)
        {
            var analyticsEnabled = Configuration.Settings.AnalyticsEnabled;

            if (!analyticsEnabled)
                return;
            if (!Tracker.IsActive)
                Tracker.StartTracking();
            if (Tracker.Current.CurrentPage == null)
                return;
            ((IPageContext)Tracker.Current.CurrentPage).Register(eventData);
        }
    }
}