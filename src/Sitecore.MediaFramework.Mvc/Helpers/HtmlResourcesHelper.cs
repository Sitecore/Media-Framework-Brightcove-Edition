namespace Sitecore.MediaFramework.Mvc.Helpers
{
  using System.Collections.Generic;
  using System.Text;
  using System.Web;
  using System.Web.Mvc;

  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Mvc.Data;
  using Sitecore.MediaFramework.Mvc.Pipelines.GetDefaultResources;
  using Sitecore.Mvc.Presentation;

  public class HtmlResourcesHelper : IHtmlResourcesHelper
  {
    public virtual HtmlResourcesContext CreateResourcesContext(HttpContextBase httpContext, string contextKey, HtmlResources resources = null)
    {
      return new HtmlResourcesContext(httpContext, contextKey, resources);
    }

    public virtual Stack<HtmlResourcesContext> GetHtmlResources(HttpContextBase httpContext, string contextKey)
    {
      var stack = httpContext.Items[contextKey] as Stack<HtmlResourcesContext>;
      if (stack == null)
      {
        stack = new Stack<HtmlResourcesContext>();
        httpContext.Items[contextKey] = stack;
      }
      return stack;
    }

    public virtual IHtmlString Render(Stack<HtmlResourcesContext> resources)
    {
      if (resources == null || resources.Count == 0)
      {
        return MvcHtmlString.Empty;
      }

      var builder = new StringBuilder();

      int count = resources.Count;
      for (int i = 0; i < count; i++)
      {
        var scriptContext = resources.Pop();

        builder.Append(scriptContext.Render().ToHtmlString());
      }

      return new HtmlString(builder.ToString());
    }

    public virtual IHtmlString RenderItemId(PageContext pageContext)
    {
      if (pageContext == null)
      {
        return MvcHtmlString.Empty;
      }

      Item item = pageContext.Item;
      if (item == null)
      {
        return MvcHtmlString.Empty;
      }
      return new HtmlString(string.Format("<input id=\"{0}\" name=\"{0}\" type=\"hidden\" value=\"{1}\">", Constants.ItemIdKey, item.ID.ToShortID()));
    }

    public virtual void RegisterDefaultResources(HttpContextBase httpContext)
    {
      Assert.ArgumentNotNull(httpContext, "httpContext");

      var head = this.GetHtmlResources(httpContext, Constants.HeadResourcesKey);
      var body = this.GetHtmlResources(httpContext, Constants.BodyResourcesKey);

      var resourcesArgs = GetDefaultResourcesPipeline.Run();
      if (head.Count > 0 || body.Count > 0)
      {
        using (this.CreateResourcesContext(httpContext, Constants.HeadResourcesKey, resourcesArgs.Head))
        {
        }
        using (this.CreateResourcesContext(httpContext, Constants.BodyResourcesKey, resourcesArgs.Body))
        {
        }
      }

      using (this.CreateResourcesContext(httpContext, Constants.BodyResourcesKey, resourcesArgs.AllPageBody))
      {
      }
    }
  }
}