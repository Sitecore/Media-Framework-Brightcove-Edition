namespace Sitecore.MediaFramework.Mvc.Pipelines.GetDefaultResources
{
  using Sitecore.MediaFramework.Mvc.Data;
  using Sitecore.Pipelines;

  public class GetDefaultResourcesArgs : PipelineArgs
  {
    public HtmlResources Head { get; set; }
    public HtmlResources Body { get; set; }
    public HtmlResources AllPageBody { get; set; }

    public GetDefaultResourcesArgs()
    {
      this.Head = new HtmlResources();
      this.Body = new HtmlResources();
      this.AllPageBody = new HtmlResources();
    }
  }
}