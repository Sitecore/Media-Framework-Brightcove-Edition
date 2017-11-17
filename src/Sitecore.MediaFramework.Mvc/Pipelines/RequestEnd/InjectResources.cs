
namespace Sitecore.MediaFramework.Mvc.Pipelines.RequestEnd
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Web.Routing;

  using Sitecore.MediaFramework.Mvc.IO;
  using Sitecore.MediaFramework.Mvc.Text;
  using Sitecore.Mvc.Pipelines.Request.RequestEnd;
  using Sitecore.Mvc.Presentation;

  public class InjectResources : RequestEndProcessor
  {
    protected List<IHtmlUpdater> Updaters { get; set; }

    public InjectResources()
    {
      this.Updaters = new List<IHtmlUpdater>();
    }

    public override void Process(RequestEndArgs args)
    {
      try
      {
        PageContext pageContext = args.PageContext;
        if (pageContext == null)
        {
          return;
        }

        RequestContext requestContext = pageContext.RequestContext;
        Stream stream = requestContext.HttpContext.Response.Filter;

        if (stream != null && this.Updaters.Count > 0)
        {
          requestContext.HttpContext.Response.Filter = new HtmlUpdateFilter(stream, this.Updaters);
        }
      }
      catch (InvalidOperationException)
      {
      }
    }
  }
}