
namespace Sitecore.MediaFramework.Pipelines.Common
{
  using Sitecore.Globalization;
  using Sitecore.Jobs;
  using Sitecore.Pipelines;

  /// <summary>
  /// Job Status
  /// </summary>
  public class JobStatus
  {
    /// <summary>
    /// Process
    /// </summary>
    /// <param name="args"></param>
    public virtual void Process(PipelineArgs args)
    {
      IItemArgs itemArgs = args as IItemArgs;

      if (itemArgs == null || itemArgs.Item == null)
      {
        return;
      }

      Job job = this.GetJob();

      if (job != null)
      {
        ++job.Status.Processed;
        job.Status.Messages.Add(Translate.Text(Translations.JobStatus,itemArgs.Item.Name, job.Status.Processed));
      }
    }

    protected virtual Job GetJob()
    {
      Job job = Context.Job;

      if (job == null)
        return null;

      return job.Options.CustomData as Job ?? job;
    }
  }
}