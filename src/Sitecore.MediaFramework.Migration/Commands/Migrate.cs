
namespace Sitecore.MediaFramework.Migration.Commands
{
  using System.Threading;

  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Jobs;
  using Sitecore.Shell.Framework.Commands;
  using Sitecore.Shell.Framework.Jobs;
  using Sitecore.Web.UI.Sheer;

  public class Migrate : Command
  {
    public override void Execute(CommandContext context)
    {
      Assert.ArgumentNotNull(context, "context");
      Assert.ArgumentNotNull(context.Items, "context.Items");

      if (context.Items.Length <= 0 || context.Items[0] == null)
      {
        return;
      }

      if (MigrationManager.Providers[context.Parameters["provider"]] == null)
      {
        SheerResponse.Alert(Translations.MigrationProviderIsNotDefined);
        return;
      }

      var jobOptions = new JobOptions("Migrate Content", "UI", Client.Site.Name, this, "Run", new object[] { context })
      {
        Priority = ThreadPriority.Normal,
        ContextUser = Context.User,
        ClientLanguage = Context.Language,
        EnableSecurity = true
      };

      var longRunningOptions = new LongRunningOptions(JobManager.Start(jobOptions).Handle.ToString())
      {
        Title = "Migrate Content",
        Icon = this.GetIcon(context, string.Empty),
        Threshold = 200,
        Message = string.Empty
      };

      longRunningOptions.ShowModal();
    }

    public override CommandState QueryState(CommandContext context)
    {
      return MediaFrameworkContext.EnableMigration ? CommandState.Enabled : CommandState.Hidden;
    }

    protected virtual void Run(CommandContext context)
    {
      Assert.ArgumentNotNull(context, "context");
      Assert.ArgumentNotNull(context.Items, "context.Items");
      Assert.ArgumentNotNull(context.Parameters, "context.Parameters");

      Item item = context.Items[0];
      bool deep = MainUtil.GetBool(context.Parameters["deep"], false);

      MigrationProvider provider = MigrationManager.Providers[context.Parameters["provider"]];

      if (provider != null)
      {
        provider.MigrateContent(item, deep);
      }
    }
  }
}
