
namespace Sitecore.MediaFramework.Migration
{
  using System;
  using System.Collections.Generic;
  using System.Configuration.Provider;
  using System.Xml;

  using Sitecore.Caching;
  using Sitecore.Configuration;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Globalization;
  using Sitecore.Integration.Common.Extensions;
  using Sitecore.Jobs;
  using Sitecore.MediaFramework.Diagnostics;
  using Sitecore.MediaFramework.Migration.Migrators;

  public class MigrationProvider : ProviderBase
  {
    public List<IMigrator> Migrators { get; protected set; }

    public MigrationProvider()
    {
      this.Migrators = new List<IMigrator>();
    }

    public void AddMigrator(XmlNode configNode)
    {
      IMigrator migrator = Factory.CreateObject(configNode, false) as IMigrator;
      if (migrator != null)
      {
        this.Migrators.Add(migrator);
      }
      else
      {
        LogHelper.Warn("Migrator could not be resolved", this);
      }
    }

    public virtual void MigrateContent(Item rootItem, bool deep)
    {
      Assert.ArgumentNotNull(rootItem, "rootItem");

      this.BeforeContentMigration(rootItem, deep);
      
      foreach (var item in this.GetItems(rootItem, deep))
      {
        if (item == null)
        {
          continue;
        }

        LogHelper.Info(string.Format("{0} item migration", item.Uri), this);

        try
        {
          this.MigrateItem(item);
          this.UpdateJobStatus(item);
        }
        catch (Exception ex)
        {
          LogHelper.Error("Item migration process failed. Item:"+item.Uri, this, ex);
        }
      }

      this.AfterContentMigration(rootItem, deep);
    }

    public virtual void MigrateItem(Item item)
    {
      Assert.ArgumentNotNull(item, "item");

      foreach (var migrator in this.Migrators)
      {
        try
        {
          migrator.MigrateItem(item);
        }
        catch (Exception ex)
        {
          LogHelper.Error(string.Format("Migration failed. Item:{0};Migrator:{1}", item.Uri, migrator.GetType().Name), this, ex);
        }
      }
    }

    public virtual IEnumerable<Item> GetItems(Item rootItem, bool isDeep)
    {
      Assert.ArgumentNotNull(rootItem, "rootItem");

      yield return rootItem;
      if (isDeep)
      {
        foreach (Item item in rootItem.GetDescendants())
        {
          yield return item;
        }
      }
    }

    protected virtual void BeforeContentMigration(Item rootItem, bool deep)
    {
      Assert.ArgumentNotNull(rootItem, "rootItem");

      CacheManager.ClearAllCaches();
    }

    protected virtual void AfterContentMigration(Item rootItem, bool deep)
    {
      CacheManager.ClearAllCaches();
    }

    protected virtual void UpdateJobStatus(Item item, string message = null)
    {
      if (item == null)
      {
        return;
      }

      Job job = Context.Job;

      if (job != null)
      {
        job.Status.Processed++;

        if (string.IsNullOrEmpty(message))
        {
          message = Translate.Text(Translations.JobStatus, item.Name, job.Status.Processed);
        }

        job.Status.Messages.Add(message);
      }
    }
  }
}