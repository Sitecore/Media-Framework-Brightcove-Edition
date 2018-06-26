namespace Sitecore.MediaFramework.Rules.Analytics.Actions
{
  using Sitecore.Diagnostics;
  using Sitecore.Rules.Actions;

  public class TriggerPlaybackStarted<T> : RuleAction<T>
    where T : PlaybackRuleContext
  {
    public override void Apply(T ruleContext)
    {
      Assert.ArgumentNotNull(ruleContext, "ruleContext");

      ruleContext.AddPlaybackEvent(PlaybackEvents.PlaybackStarted.ToString(), replace:true);
    }
  }
}