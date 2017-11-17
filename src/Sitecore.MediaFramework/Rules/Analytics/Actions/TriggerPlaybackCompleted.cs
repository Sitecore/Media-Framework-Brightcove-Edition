namespace Sitecore.MediaFramework.Rules.Analytics.Actions
{
  using Sitecore.Diagnostics;
  using Sitecore.Rules.Actions;

  public class TriggerPlaybackCompleted<T> : RuleAction<T> where T : PlaybackRuleContext
  {
    public override void Apply(T ruleContext)
    {
      Assert.ArgumentNotNull(ruleContext, "ruleContext");

      ruleContext.AddPlaybackEvent(PlaybackEvents.PlaybackCompleted.ToString(), replace:true);
    }
  }
}