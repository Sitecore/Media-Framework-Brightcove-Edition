namespace Sitecore.MediaFramework.Scopes
{
  using System.Collections.Generic;
  using System.Xml;

  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Account;
  using Sitecore.Reflection;
  using Sitecore.Xml;

  public class ScopeExecuteConfiguration : IInitializable
  {
    public ScopeExecuteConfiguration()
    {
      this.Scope = new List<string>();
      this.AccountIds = new List<ID>();
    }

    public string Name { get; set; }

    public string AccountTemplate { get; set; }

    public List<string> Scope { get; set; } 

    public List<ID> AccountIds { get; set; }
    
    public void AddAccountId(string id)
    {
      if (ID.IsID(id))
      {
        this.AddAccountId(new ID(id));
      }
    }

    public void AddAccountId(ID id)
    {
      this.AccountIds.Add(id);
    }

    public virtual IEnumerable<Item> GetAccountList(Database database)
    {
      if (this.AccountIds.Count > 0)
      {
        return AccountManager.GetAccountsByIds(database, this.AccountIds);
      }

      if (ID.IsID(this.AccountTemplate))
      {
        return AccountManager.GetAccountsByTemplate(database, new ID(this.AccountTemplate));
      }

      return new List<Item>(0);
    }

    public void Initialize(XmlNode configNode)
    {
      this.AssignProperties = true;

      this.Name = XmlUtil.GetAttribute("name", configNode);
    }

    public bool AssignProperties { get; private set; }
  }
}