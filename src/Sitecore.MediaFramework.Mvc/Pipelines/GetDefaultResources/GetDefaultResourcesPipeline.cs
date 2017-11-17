
namespace Sitecore.MediaFramework.Mvc.Pipelines.GetDefaultResources
{
  using Sitecore.Diagnostics;
  using Sitecore.Pipelines;

  public static class GetDefaultResourcesPipeline
  {
    public static GetDefaultResourcesArgs Run()
    {
      return Run(new GetDefaultResourcesArgs());
    }

    public static GetDefaultResourcesArgs Run(GetDefaultResourcesArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      CorePipeline.Run("mediaFramework.mvc.getDefaultResources", args);
      return args;
    }
  }
}