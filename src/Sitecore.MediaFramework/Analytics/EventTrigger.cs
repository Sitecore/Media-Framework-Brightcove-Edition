// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventTrigger.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   The event trigger.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Analytics
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;

  using Sitecore.Analytics;
  using Sitecore.Analytics.Data;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Diagnostics;
  using Sitecore.MediaFramework.Pipelines.Analytics;

  /// <summary>
  /// The event trigger.
  /// </summary>
  public abstract class EventTrigger : IEventTrigger
  {
    protected Dictionary<Tuple<ID, string>, Func<PlayerEventArgs, PageEventData>> Events { get; set; }

    protected EventTrigger()
    {
      this.Events = new Dictionary<Tuple<ID, string>, Func<PlayerEventArgs, PageEventData>>();
    }

    /// <summary>
    /// Registeres player events.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public virtual void Register(PlayerEventArgs args)
    {
      Func<PlayerEventArgs, PageEventData> getPageEventData;

      var key = Tuple.Create(args.Properties.Template, args.EventName);

      if (this.Events.TryGetValue(key, out getPageEventData))
      {
        this.TriggerEvent(getPageEventData(args));
      }
      else
      {
        LogHelper.Warn("Provider does not handle this event. Key:" + key, this);
      }
    }

    public abstract void InitEvents();

    /// <summary>
    /// Trigger an event or a goal.
    /// </summary>
    /// <param name="eventData">
    /// The page event data.
    /// </param>
    protected virtual void TriggerEvent(PageEventData eventData)
    {
      if (Sitecore.Configuration.Settings.GetBoolSetting("Xdb.Enabled", false))
      {
        if (!Tracker.IsActive)
        {
          Tracker.StartTracking();
        }

        if (Tracker.Current.CurrentPage != null)
        {
          Tracker.Current.CurrentPage.Register(eventData);
        }
      }
    }

    /// <summary>
    /// Adds event for triggering.
    /// </summary>
    /// <param name="templateId">
    /// The item template ID
    /// </param>
    /// <param name="eventName">
    /// The event name.
    /// </param>
    /// <param name="text">
    /// The text.
    /// </param>
    protected virtual void AddEvent(ID templateId, string eventName, string text)
    {
      Assert.ArgumentNotNull(templateId, "templateId");
      Assert.ArgumentNotNullOrEmpty(eventName, "eventName");
      Assert.ArgumentNotNullOrEmpty(text, "text");

      var key = Tuple.Create(templateId, eventName);
      if (!this.Events.ContainsKey(key))
      {
        this.Events.Add(
          key,
          args =>
          new PageEventData(args.EventName)
            {
              ItemId = args.Properties.ContextItemId.Guid,
              DataKey = args.Properties.MediaId,
              Data = this.GetParameters(args),
              Text = text
            });
      }
      else
      {
        LogHelper.Warn("Duplicate Analytics event mapping. Key:" + key, this);
      }
    }

    /// <summary>
    /// Gets parameters.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    protected virtual string GetParameters(PlayerEventArgs args)
    {
      return args.Properties.MediaName.Replace('|','_') + "|" + args.Properties.MediaLength.ToString(CultureInfo.InvariantCulture) + "|" + args.Properties.EventParameter;
    }
  }
}