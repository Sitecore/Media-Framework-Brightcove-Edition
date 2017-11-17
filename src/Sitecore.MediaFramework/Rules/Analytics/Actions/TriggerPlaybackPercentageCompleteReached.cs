namespace Sitecore.MediaFramework.Rules.Analytics.Actions
{
  using System.Globalization;

  using Sitecore.Diagnostics;
  using Sitecore.Rules.Actions;

  public class TriggerPlaybackPercentageCompleteReached<T> : RuleAction<T>
    where T : PlaybackRuleContext
  {
    public int PercentagePoint { get; set; }

    public override void Apply(T ruleContext)
    {
      Assert.ArgumentNotNull(ruleContext, "ruleContext");

      if (this.PercentagePoint == 0)
      {
        return;
      }

      ruleContext.AddPlaybackEvent(PlaybackEvents.PlaybackChanged.ToString(), this.PercentagePoint.ToString(CultureInfo.InvariantCulture) + '%');
    }
  }
}