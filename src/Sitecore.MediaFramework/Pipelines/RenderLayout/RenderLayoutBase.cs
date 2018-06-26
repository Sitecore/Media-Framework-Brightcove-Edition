namespace Sitecore.MediaFramework.Pipelines.RenderLayout
{
  using System.Collections.Generic;
  using System.Web.UI;

  using Sitecore.Diagnostics;
  using Sitecore.Pipelines.RenderLayout;

  public abstract class RenderLayoutBase
  {
    protected RenderLayoutBase()
    {
      this.FilterSites = new List<string>();
    }
    
    public List<string> FilterSites { get; protected set; }

    protected IRootControlResolver RootControlResolver { get; set; }

    public void AddFilterSite(string siteName)
    {
      Assert.ArgumentNotNullOrEmpty(siteName, "siteName");

      if (!this.FilterSites.Contains(siteName))
      {
        this.FilterSites.Add(siteName);
      }
    }

    public virtual void Process(RenderLayoutArgs args)
    {
      if (this.IsValidContextSite(args))
      {
        this.Render(args);
      }
    }

    public virtual void Render(RenderLayoutArgs args)
    {
      Control controlToRender = this.GetControlToRender();
      if (controlToRender == null)
      {
        return;
      }

      Control root = this.RootControlResolver != null? this.RootControlResolver.GetRootControl() : null;
      if (root != null)
      {
        root.Controls.Add(controlToRender);
      }
    }

    protected abstract Control GetControlToRender();

    /// <summary>
    /// Is Valid Context Site
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    protected virtual bool IsValidContextSite(RenderLayoutArgs args)
    {
      var siteContext = Context.Site;

      return siteContext != null && !this.FilterSites.Contains(siteContext.Name);
    }
  }
}