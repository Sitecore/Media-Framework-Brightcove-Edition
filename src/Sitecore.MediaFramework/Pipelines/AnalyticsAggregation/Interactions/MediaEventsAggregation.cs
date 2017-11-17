// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaEventsAggregation.cs" company="Sitecore A/S">
//   Copyright (C) 2015 by Sitecore A/S
// </copyright>
// <summary>
//   Defines the MediaEventsAggregation type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Pipelines.AnalyticsAggregation.Interactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sitecore.Analytics.Aggregation.Pipeline;
    using Sitecore.Analytics.Core;
    using Sitecore.Analytics.Model;
    using Sitecore.ExperienceAnalytics.Api.Encoding;
    using Sitecore.MediaFramework.Data.Analytics;

    public class MediaEventsAggregation : InteractionAggregationPipelineProcessor
    {
        protected override void OnProcess(InteractionAggregationPipelineArgs args)
        {
            var dimension = args.Context.Results.GetDimension<MediaFrameworkMedia>();
            var fact = args.Context.Results.GetFact<MediaFrameworkEvents>();

            var visit = args.Context.Interaction;

            DateTime date = args.DateTimeStrategy.Translate(visit.StartDateTime);

            foreach (var pageEvent in visit.Events)
            {
                var mediaEvent = MediaEventData.Parse(pageEvent);
                if (mediaEvent == null)
                {
                    continue;
                }
                
                Hash128 mediaId = dimension.AddValue(mediaEvent);
                var encoder = new Hash32Encoder();
                string hashName = encoder.Encode((visit.VenueId ?? Guid.Empty).ToString());

                var key = new MediaFrameworkEventsKey
                {
                    Date = date,
                    MediaId = mediaId,
                    PageEventDefinitionId = pageEvent.DefinitionId,
                    EventParameter = mediaEvent.EventParameter,
                    SiteNameId = int.Parse(hashName)
                };

                var value = new MediaFrameworkEventsValue { Count = 1 };

                fact.Emit(key, value);
            }
        }

        protected virtual List<PageEventData> GetPageEvents(VisitData visit)
        {
            return new List<PageEventData>(
              visit.Pages
                .Where(page => page.PageEvents != null)
                .SelectMany(page => page.PageEvents)
                .Where(pageEvent => MediaFrameworkContext.IsMediaEvent(pageEvent.PageEventDefinitionId)));
        }
    }
}