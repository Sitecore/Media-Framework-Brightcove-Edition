namespace Sitecore.MediaFramework.Rules.Analytics.Actions
{
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Rules.Actions;

  public class AddMediaItemEvents<T> : RuleAction<T> where T : PlaybackRuleContext
  {
    public override void Apply(T ruleContext)
    {
      Assert.ArgumentNotNull(ruleContext, "ruleContext");
      Assert.ArgumentNotNull(ruleContext.Item, "ruleContext.Item");

      MultilistField eventsField = ruleContext.Item.Fields[FieldIDs.MediaElement.Events];

      if (eventsField == null)
      {
        return;
      }

      foreach (Item item in eventsField.GetItems())
      {
        string eventName = item[FieldIDs.PlaybackEvent.PageEvent];
        if (eventName.Length > 0)
        {
          string parameter = item[FieldIDs.PlaybackEvent.Parameter];
          ruleContext.AddPlaybackEvent(eventName, parameter, parameter.Length == 0);
        }
      }
    }
  }
}