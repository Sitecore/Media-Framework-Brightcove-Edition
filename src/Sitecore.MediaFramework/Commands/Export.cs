namespace Sitecore.MediaFramework.Commands
{
  using System;
  using System.Threading;

  using Sitecore.Jobs;
  using Sitecore.MediaFramework.Pipelines.MediaExport;
  using Sitecore.Pipelines;
  using Sitecore.Shell.Framework.Commands;
  using Sitecore.Shell.Framework.Jobs;

  [Serializable]
  public class Export : Command
  {
    public override void Execute(CommandContext context)
    {
      JobOptions jobOptions = new JobOptions("Export MediaFramework Content", "UI", Client.Site.Name, this, "Run", new object[] { context })
      {
        Priority = ThreadPriority.Normal,
        ContextUser = Context.User,
        ClientLanguage = Context.Language,
        EnableSecurity = true
      };

      LongRunningOptions longRunningOptions = new LongRunningOptions(JobManager.Start(jobOptions).Handle.ToString())
      {
        Title = "Export MediaFramework Content",
        Icon = this.GetIcon(context, string.Empty),
        Threshold = 200,
        Message = string.Empty
      };

      longRunningOptions.ShowModal();
    }

    public override CommandState QueryState(CommandContext context)
    {
      return MediaFrameworkContext.IsExportAllowed() ? CommandState.Enabled : CommandState.Hidden;
    }

    protected virtual void Run(CommandContext context)
    {
      MediaExportPipeline.Run(new PipelineArgs());
    }
  }
}