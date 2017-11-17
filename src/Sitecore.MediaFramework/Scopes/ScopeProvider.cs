namespace Sitecore.MediaFramework.Scopes
{
  using System;
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using System.Configuration.Provider;
  using System.Threading;

  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Jobs;
  using Sitecore.MediaFramework.Account;
  using Sitecore.MediaFramework.Diagnostics;
  using Sitecore.MediaFramework.Schedulers;

  public class ScopeProvider : ProviderBase
  {
    protected readonly ConcurrentDictionary<Tuple<ID, string>, Handle> ScopeJobs;

    public ScopeProvider()
    {
      this.ScopeJobs = new ConcurrentDictionary<Tuple<ID, string>, Handle>();
    }

    public virtual ScopeExecuteConfiguration GetExecuteConfiguration(string name)
    {
      Assert.ArgumentNotNullOrEmpty(name, "name");

      ScopeExecuteConfiguration result = MediaFrameworkContext.GetScopeExecuteConfiguration(name);

      if (result == null)
      {
        LogHelper.Warn("Execute configuration could not be resolved. Name:" + name, this);
      }

      return result;
    }

    public virtual void Execute(Database database, string name, Action<Item, string> scopeAction, IEnumerable<ID> exactAccounts = null)
    {
      Assert.ArgumentNotNullOrEmpty(name, "name");

      ScopeExecuteConfiguration executeConfiguration = this.GetExecuteConfiguration(name);

      if (executeConfiguration != null)
      {
        this.Execute(database, executeConfiguration, scopeAction, exactAccounts);
      }
    }

    public virtual void Execute(Database database, ScopeExecuteConfiguration executeConfiguration, Action<Item, string> scopeAction, IEnumerable<ID> exactAccounts = null)
    {
      Assert.ArgumentNotNull(executeConfiguration, "executeConfiguration");

      if (exactAccounts != null)
      {
        executeConfiguration.AccountIds.Clear();
        executeConfiguration.AccountIds.AddRange(exactAccounts);
      }

      foreach (Item accountItem in executeConfiguration.GetAccountList(database))
      {
        if (AccountManager.IsValidAccount(accountItem))
        {
          foreach (string scope in executeConfiguration.Scope)
          {
            try
            {
              RunAction(scopeAction, accountItem, scope);
            }
            catch (Exception ex)
            {
              LogHelper.Error("Error while executing scope action", this, ex);
            }
          }
        }
        else
        {
          LogHelper.Warn(Translations.AccountItemValidationFailed + " Account Id:" + accountItem.ID, this);
        }
      }
    }

    protected virtual void RunAction(Action<Item, string> scopeAction, Item accountItem, string scope)
    {
      Assert.ArgumentNotNull(scopeAction, "scopeAction");
      Assert.ArgumentNotNull(accountItem, "accountItem");
      Assert.ArgumentNotNullOrEmpty(scope, "scope");
         
      Handle jobHandle;
      var key = Tuple.Create(accountItem.ID, scope);

      try
      {           
        if (this.ScopeJobs.TryGetValue(key, out jobHandle))
        {
          var job = JobManager.GetJob(jobHandle);

          LogHelper.Debug(string.Format("Scope job({0}) already started", job.Name), this);
          job.Wait();
        }
        else
        {
          this.RunAction(scopeAction, accountItem, scope, key);
        }   
      }
      catch (Exception ex)
      {
        LogHelper.Error("Error while executing scope action", this, ex);
      }
      finally
      {
        this.ScopeJobs.TryRemove(key, out jobHandle);
      }
    }

    protected virtual void RunAction(Action<Item, string> scopeAction, Item accountItem, string scope, Tuple<ID, string> jobName)
    {
      var options = new JobOptions(
        string.Format("MediaFramework_ScopeAction_{0}_{1}", accountItem.ID, scope),
        "MediaFramework",
        Context.Site.Name,
        this,
        "Run",
        new object[] { scopeAction, accountItem, scope })
      {
        Priority = ThreadPriority.Normal,
        ContextUser = Context.User,
        ClientLanguage = Context.Language,
        EnableSecurity = true,
        CustomData = Context.Job,
        WriteToLog = false
      };
    
      var job = JobManager.Start(options);

      LogHelper.Debug(string.Format("Scope job({0}) has been started", job.Name), this);

      this.ScopeJobs.TryAdd(jobName, job.Handle);

      job.Wait();  
    }

    public void Run(Action<Item, string> scopeAction, Item accountItem, string scope)
    {
      scopeAction(accountItem, scope);
    }
  }
}