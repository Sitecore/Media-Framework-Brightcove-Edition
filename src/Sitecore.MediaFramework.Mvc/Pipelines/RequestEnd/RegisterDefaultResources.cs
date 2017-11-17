namespace Sitecore.MediaFramework.Mvc.Pipelines.RequestEnd
{
  using System;
  using Sitecore.MediaFramework.Mvc.Extentions;
  using Sitecore.Mvc.Pipelines.Request.RequestEnd;
  using Sitecore.Mvc.Presentation;

  public class RegisterDefaultResources : RequestEndProcessor
  {
    public override void Process(RequestEndArgs args)
    {
      try
      {
        PageContext pageContext = args.PageContext;
        if (pageContext == null)
        {
          return;
        }

        pageContext.RequestContext.HttpContext.RegisterDefaultResources();
      }
      catch (InvalidOperationException)
      {
      }
    }
  }
}