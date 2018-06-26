// Decompiled with JetBrains decompiler
// Type: Sitecore.MediaFramework.Dimensions.ByStreamedVideo
// Assembly: Sitecore.MediaFramework, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 754459D4-885A-4395-B63B-17372D2F05DD
// Assembly location: C:\Websites\brightcove82\Website\bin\Sitecore.MediaFramework.dll

using Sitecore.Analytics.Aggregation.Data.Model;
using Sitecore.Analytics.Model;
using Sitecore.ExperienceAnalytics.Aggregation.Data.Model;
using Sitecore.ExperienceAnalytics.Aggregation.Data.Schema;
using Sitecore.ExperienceAnalytics.Aggregation.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.MediaFramework.Dimensions
{
    public class ByStreamedVideo : DimensionBase
    {
        public ByStreamedVideo(Guid dimensionId)
          : base(dimensionId)
        {
        }

        public override IEnumerable<DimensionData> GetData(IVisitAggregationContext context)
        {
            foreach (PageData page in context.Visit.Pages)
            {
                PageData data = page;
                IEnumerable<bool> result = data.PageEvents.Select<PageEventData, bool>((Func<PageEventData, bool>)(p => p.PageEventDefinitionId == ItemIDs.PageEvents.PlaybackStarted.ToGuid()));
                var startedVideos = data.PageEvents.Where(p => p.PageEventDefinitionId == ItemIDs.PageEvents.PlaybackStarted.ToGuid()).GroupBy(p => p.Data).Select(group => new
                {
                    Metric = group.Key,
                    Count = group.Count<PageEventData>()
                }).OrderBy(x => x.Metric);
                var endedVideos = data.PageEvents.Where(p => p.PageEventDefinitionId == ItemIDs.PageEvents.PlaybackCompleted.ToGuid()).GroupBy(p => p.Data).Select(group => new
                {
                    Metric = group.Key,
                    Count = group.Count<PageEventData>()
                }).OrderBy(x => x.Metric);
                var enumerator1 = startedVideos.GetEnumerator();
                SegmentMetricsValue calculations;
                while (enumerator1.MoveNext())
                {
                    var video = enumerator1.Current;
                    calculations = this.CalculateCommonMetrics(context, 0);
                    calculations.Visits = video.Count;
                    yield return new DimensionData()
                    {
                        DimensionKey = video.Metric,
                        MetricsValue = calculations
                    };
                    video = null;
                }
                enumerator1 = null;
                var enumerator2 = endedVideos.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    var video = enumerator2.Current;
                    calculations = this.CalculateCommonMetrics(context, 0);
                    calculations.Count = video.Count;
                    yield return new DimensionData()
                    {
                        DimensionKey = video.Metric,
                        MetricsValue = calculations
                    };
                    video = null;
                }
                enumerator2 = null;
                result = (IEnumerable<bool>)null;
                startedVideos = null;
                endedVideos = null;
                calculations = (SegmentMetricsValue)null;
                data = (PageData)null;
            }
        }
    }
}
