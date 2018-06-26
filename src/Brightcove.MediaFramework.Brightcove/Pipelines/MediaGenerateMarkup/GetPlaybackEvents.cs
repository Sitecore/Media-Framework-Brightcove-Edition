using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brightcove.MediaFramework.Brightcove.Configuration;
using Sitecore;
using Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup;

namespace Brightcove.MediaFramework.Brightcove.Pipelines.MediaGenerateMarkup
{
    public class GetPlaybackEvents : Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup.GetPlaybackEvents
    {
        protected override bool CheckState(MediaGenerateMarkupArgs args)
        {
            if (args.MarkupType == MarkupType.Html && Settings.AnalyticsEnabled)
                return !Context.PageMode.IsExperienceEditorEditing;
            else
                return false;
        }
    }
}
