namespace Sitecore.MediaFramework.Pipelines.RenderLayout
{
  using System.Web.UI;

  public interface IRootControlResolver
  {
    Control GetRootControl();
  }
}