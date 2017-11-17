namespace Sitecore.MediaFramework.Rules.Analytics.Actions
{
  using Sitecore.Diagnostics;
  using Sitecore.Rules.Actions;

  public class TriggerPlaybackError<T> : RuleAction<T>
    where T : PlaybackRuleContext
  {
    public override void Apply(T ruleContext)
    {
      Assert.ArgumentNotNull(ruleContext, "ruleContext");

      ruleContext.AddPlaybackEvent(PlaybackEvents.PlaybackError.ToString(), replace: true);
    }
  }
}