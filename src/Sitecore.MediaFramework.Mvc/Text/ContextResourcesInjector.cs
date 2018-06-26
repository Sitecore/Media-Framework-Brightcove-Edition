namespace Sitecore.MediaFramework.Mvc.Text
{
  using System.Text;
  using System.Web;

  using Sitecore.MediaFramework.Mvc.Extentions;

  public class ContextResourcesInjector : HtmlUpdaterBase
  {
    public override bool UpdateHtml(StringBuilder html)
    {
      HttpContext current = HttpContext.Current;
      if (current == null)
      {
        return false;
      }

      var contextWrapper = new HttpContextWrapper(current);

      var head = contextWrapper.GetHtmlResources(Constants.HeadResourcesKey).Render().ToHtmlString();
      var body = contextWrapper.GetHtmlResources(Constants.BodyResourcesKey).Render().ToHtmlString();
      
      if (string.IsNullOrEmpty(head) && string.IsNullOrEmpty(body))
      {
        return false;
      }

      int headIndex = this.IndexOfHeadEnd(html);
      if (headIndex > 0)
      {
        html.Insert(headIndex, head);
      }

      int bodyIndex = this.IndexOfBodyEnd(html);
      if (bodyIndex > 0)
      {
        html.Insert(bodyIndex, body);
      }

      return true;
    }
  }
}