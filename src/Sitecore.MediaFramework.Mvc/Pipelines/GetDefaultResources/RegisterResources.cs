namespace Sitecore.MediaFramework.Mvc.Pipelines.GetDefaultResources
{
  using Sitecore.MediaFramework.Mvc.Data;

  public class RegisterResources : GetDefaultResourcesProcessor
  {
    public HtmlResources Head { get; set; }
    public HtmlResources Body { get; set; }

    public override void Process(GetDefaultResourcesArgs args)
    {
      if (this.Head != null)
      {
        args.Head.UnionWith(this.Head);
      }

      if (this.Body != null)
      {
        args.Body.UnionWith(this.Body);
      }
    }
  }
}