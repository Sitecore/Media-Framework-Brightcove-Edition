namespace Sitecore.MediaFramework.Mvc.Pipelines.GetDefaultResources
{
  using System.Collections.Generic;

  public class FilterBySite : GetDefaultResourcesProcessor
  {
    protected List<string> Sites { get; set; }

    public FilterBySite()
    {
      this.Sites = new List<string>();
    }

    public override void Process(GetDefaultResourcesArgs args)
    {
      if (!this.IsValidContextSite())
      {
        args.AbortPipeline();
      }
    }

    protected virtual bool IsValidContextSite()
    {
      var siteContext = Context.Site;

      return siteContext != null && !this.Sites.Contains(siteContext.Name);
    }
  }
}