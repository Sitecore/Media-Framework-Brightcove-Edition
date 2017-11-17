namespace Sitecore.MediaFramework.Commands
{
  using System;
  using System.Threading;

  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Integration.Common.Commands;
  using Sitecore.Jobs;
  using Sitecore.MediaFramework.Account;
  using Sitecore.MediaFramework.Diagnostics;
  using Sitecore.MediaFramework.Scopes;
  using Sitecore.Shell.Framework.Commands;
  using Sitecore.Shell.Framework.Jobs;
  using Sitecore.Web.UI.Sheer;

  public abstract class ManualCommandBase : RuleBasedCommand
  {
    protected abstract Action<Item, string> Action { get; }

    protected abstract string JobName { get; }

    protected virtual string Title
    {
      get
      {
        return this.JobName;
      }
    }

    protected override CommandState DefaultCommandState
    {
      get
      {
        return CommandState.Hidden;
      }
    }

    public override void Execute(CommandContext context)
    {
      if (context == null || context.Items.Length <= 0)
      {
        return;
      }

      Item item = context.Items[0];
      if (item == null)
      {
        LogHelper.Error("Error while executing manual operation. Context item is null.", this);
        return;
      }

      Item accountItem = AccountManager.GetAccountItemForDescendant(item);
      if (accountItem == null)
      {
        LogHelper.Error("Error while executing manual operation. An Account item cannot be found.", this);
        return;
      }

      if (!AccountManager.IsValidAccount(accountItem))
      {
        SheerResponse.Alert(Translations.AccountItemValidationFailed);
        return;
      }

      context.CustomData = accountItem;

      JobOptions jobOptions = new JobOptions(this.JobName, "UI", Client.Site.Name, this, "Run", new object[] { context })
      {
        Priority = ThreadPriority.Normal,
        ContextUser = Context.User,
        ClientLanguage = Context.Language,
        EnableSecurity = true
      };

      LongRunningOptions longRunningOptions = new LongRunningOptions(JobManager.Start(jobOptions).Handle.ToString())
      {
        Title = this.Title,
        Icon = this.GetIcon(context, string.Empty),
        Threshold = 200,
        Message = string.Empty
      };
      longRunningOptions.ShowModal();
    }

    /// <summary>
    /// Query State
    /// </summary>
    /// <param name="context">
    /// The command context.
    /// </param>
    /// <returns>
    /// Command State
    /// </returns>
    protected virtual void Run(CommandContext context)
    {
      Assert.ArgumentNotNull(context, "context");
      Assert.ArgumentNotNull(context.CustomData, "context.CustomData");

      var accountItem = context.CustomData as Item;
      string scope = context.Parameters["scope"];

      if (accountItem != null && !string.IsNullOrEmpty(scope))
      {
        ScopeManager.Execute(accountItem.Database, scope, this.Action, new[] { accountItem.ID });
      }
    }
  }
}