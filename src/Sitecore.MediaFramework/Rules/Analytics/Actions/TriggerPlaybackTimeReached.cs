namespace Sitecore.MediaFramework.Rules.Analytics.Actions
{
  using System.Globalization;

  using Sitecore.Diagnostics;
  using Sitecore.Rules.Actions;

  public class TriggerPlaybackTimeReached<T> : RuleAction<T>
    where T : PlaybackRuleContext
  {
    public int TimePoint { get; set; }

    public override void Apply(T ruleContext)
    {
      Assert.ArgumentNotNull(ruleContext, "ruleContext");

      if (this.TimePoint == 0)
      {
        return;
      }

      ruleContext.AddPlaybackEvent(PlaybackEvents.PlaybackChanged.ToString(),this.TimePoint.ToString(CultureInfo.InvariantCulture));
    }
  }
}