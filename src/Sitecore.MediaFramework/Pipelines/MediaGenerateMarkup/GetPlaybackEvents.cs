namespace Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup
{
  using Sitecore.Configuration;
  using Sitecore.Data.Fields;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Account;
  using Sitecore.MediaFramework.Rules.Analytics;
  using Sitecore.Rules;

  public class GetPlaybackEvents : MediaGenerateMarkupProcessorBase
  {
    public override void Process(MediaGenerateMarkupArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.MediaItem, "args.PlayerProperties");
      Assert.ArgumentNotNull(args.AccountItem, "args.PlayerProperties");

      if (!this.CheckState(args))
      {
        return;
      }

      Field eventsField = AccountManager.GetSettingsField(args.AccountItem, FieldIDs.AccountSettings.PlaybackEventsRules);
      if (eventsField != null)
      {
        var ruleList = RuleFactory.GetRules<PlaybackRuleContext>(eventsField);

        var context = new PlaybackRuleContext { Item = args.MediaItem };

        ruleList.Run(context);

        args.PlaybackEvents = context.PlaybackEvents;
      }
    }

    protected virtual bool CheckState(MediaGenerateMarkupArgs args)
    {
      return args.MarkupType == MarkupType.Html && Sitecore.Configuration.Settings.GetBoolSetting("Xdb.Enabled", false) && !Context.PageMode.IsExperienceEditorEditing;
    }
  }
}