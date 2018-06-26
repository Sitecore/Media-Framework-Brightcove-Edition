namespace Sitecore.MediaFramework.Mvc.Pipelines.GetDefaultResources
{
  using System;
  using Sitecore.MediaFramework.Mvc.Extentions;
  using Sitecore.Mvc.Presentation;

  public class RegisterItemId : GetDefaultResourcesProcessor
  {
    public override void Process(GetDefaultResourcesArgs args)
    {
      var html = PageContext.CurrentOrNull.RenderItemId().ToHtmlString();
      if (!string.IsNullOrEmpty(html))
      {
        args.AllPageBody.HtmlBlocks.Add(html);
      }
    }
  }
}