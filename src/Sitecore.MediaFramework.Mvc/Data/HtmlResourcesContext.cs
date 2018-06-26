
namespace Sitecore.MediaFramework.Mvc.Data
{
  using System;
  using System.Collections.Generic;
  using System.Web;

  using Sitecore.Diagnostics;

  public class HtmlResourcesContext : IDisposable
  {
    protected const string ScriptBlockTemplate = "<script type='text/javascript'>{0}</script>";

    protected readonly string ContextKey;
    
    protected readonly HttpContextBase HttpContext;

    protected readonly HtmlResources Resources;

    public HtmlResourcesContext(HttpContextBase httpContext, string contextKey, HtmlResources resources = null)
    {
      Assert.ArgumentNotNull(httpContext, "httpContext");
      Assert.ArgumentNotNullOrEmpty(contextKey, "contextItems");

      this.HttpContext = httpContext;
      this.ContextKey = contextKey;
      this.Resources = resources ?? new HtmlResources();
    }

    public void AddCssUrl(string url)
    {
      this.Resources.AddCssUrl(url);
    }

    public void AddScriptUrl(string url)
    {
      this.Resources.AddScriptUrl(url);
    }

    public void AddScriptBlock(string scriptBlock)
    {
      this.AddHtmlBlock(string.Format(ScriptBlockTemplate,scriptBlock));
    }

    public void AddHtmlBlock(string html)
    {
      this.Resources.AddHtmlBlock(html);
    }

    public virtual IHtmlString Render()
    {
      return this.Resources.Render();
    }

    public virtual void Dispose()
    {
      var items = this.HttpContext.Items;
      var scriptContexts = items[this.ContextKey] as Stack<HtmlResourcesContext> ?? new Stack<HtmlResourcesContext>();

      foreach (var scriptContext in scriptContexts)
      {
        scriptContext.Resources.ExceptWith(this.Resources);
      }

      scriptContexts.Push(this);
      items[this.ContextKey] = scriptContexts;
    }
  }
}