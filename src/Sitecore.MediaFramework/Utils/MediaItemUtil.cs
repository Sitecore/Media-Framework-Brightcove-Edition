namespace Sitecore.MediaFramework.Utils
{
  using System.Linq;

  using Sitecore.Data.Items;

  public class MediaItemUtil
  {
    public static bool IsMediaElement(TemplateItem template)
    {
      if (template.ID == TemplateIDs.MediaElement)
      {
        return true;
      }

      return template.BaseTemplates.Any(IsMediaElement);
    }
  }
}