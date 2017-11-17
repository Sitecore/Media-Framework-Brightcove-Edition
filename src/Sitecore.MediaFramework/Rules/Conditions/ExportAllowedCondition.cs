namespace Sitecore.MediaFramework.Rules.Conditions
{
  using Sitecore.Rules;
  using Sitecore.Rules.Conditions;

  public class ExportAllowedCondition<T> : WhenCondition<T> where T : RuleContext
  {
    protected override bool Execute(T ruleContext)
    {
      return MediaFrameworkContext.IsExportAllowed();
    }
  }
}