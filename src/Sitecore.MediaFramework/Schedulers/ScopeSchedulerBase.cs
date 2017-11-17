namespace Sitecore.MediaFramework.Schedulers
{
  using System;
  using System.Collections.Generic;
  using System.Xml;

  using Sitecore.Configuration;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Scopes;
  using Sitecore.Security.Accounts;

  public abstract class ScopeSchedulerBase
  {
    protected ScopeSchedulerBase(string database)
      : this(database, @"sitecore\admin")
    {
    }

    protected ScopeSchedulerBase(string database, string userName)
    {
      Assert.ArgumentCondition(User.Exists(userName), "userName",userName + " is not exists");

      this.Database = database;
      this.ScopeConfigurations = new List<ScopeExecuteConfiguration>();
      this.User = User.FromName(userName, true);
    }

    public string Database { get; protected set; }

    public List<ScopeExecuteConfiguration> ScopeConfigurations { get; protected set; }

    public User User { get; set; }

    public void AddConfiguration(XmlNode node)
    {
      var scopeConfiguration = Factory.CreateObject<ScopeExecuteConfiguration>(node);
      if (scopeConfiguration != null)
      {
        this.ScopeConfigurations.Add(scopeConfiguration);
      }
    }

    public virtual void Execute()
    {
      var database = Factory.GetDatabase(this.Database);

      using (new UserSwitcher(this.User))
      {
        foreach (ScopeExecuteConfiguration scopeConfiguration in this.ScopeConfigurations)
        {
          ScopeManager.Execute(database, scopeConfiguration, this.ScopeAction);
        } 
      }
    }

    public abstract Action<Item, string> ScopeAction { get; }
  }
}