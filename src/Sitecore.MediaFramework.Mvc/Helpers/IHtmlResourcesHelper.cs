
namespace Sitecore.MediaFramework.Mvc.Helpers
{
  using System.Collections.Generic;
  using System.Web;

  using Sitecore.MediaFramework.Mvc.Data;
  using Sitecore.Mvc.Presentation;

  public interface IHtmlResourcesHelper
  {
    HtmlResourcesContext CreateResourcesContext(HttpContextBase httpContext, string contextKey, HtmlResources resources = null);

    Stack<HtmlResourcesContext> GetHtmlResources(HttpContextBase httpContext, string contextKey);

    IHtmlString Render(Stack<HtmlResourcesContext> resources);

    IHtmlString RenderItemId(PageContext pageContext);

    void RegisterDefaultResources(HttpContextBase httpContext);
  }
}
