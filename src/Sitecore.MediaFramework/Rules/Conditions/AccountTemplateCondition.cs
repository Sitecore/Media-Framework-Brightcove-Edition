namespace Sitecore.MediaFramework.Rules.Conditions
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Account;
  using Sitecore.Rules;
  using Sitecore.Rules.Conditions;

  public class AccountTemplateCondition<T>: WhenCondition<T> where T: RuleContext
  {
    public ID TemplateId { get; set; }

    protected override bool Execute(T ruleContext)
    {
      Assert.ArgumentNotNull(ruleContext, "ruleContext");
      Item item = ruleContext.Item;
      if (item == null)
      {
        return false;
      }

      Item accountItem = AccountManager.GetAccountItemForDescendant(item);

      return accountItem != null && accountItem.TemplateID == this.TemplateId;
    }
  }
}