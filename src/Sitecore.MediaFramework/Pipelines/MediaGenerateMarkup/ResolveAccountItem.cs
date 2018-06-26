namespace Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup
{
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Account;
  using Sitecore.MediaFramework.Diagnostics;

  public class ResolveAccountItem : MediaGenerateMarkupProcessorBase
  {
    public override void Process(MediaGenerateMarkupArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.MediaItem, "args.MediaItem");

      if (args.AccountItem == null)
      {
        args.AccountItem = AccountManager.GetAccountItemForDescendant(args.MediaItem);

        if (args.AccountItem == null)
        {
          LogHelper.Warn("Account item could not be found. Player markup generation will be stopped.", this);

          args.AbortPipeline();
        }
      }
    }
  }
}