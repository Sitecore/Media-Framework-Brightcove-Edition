
namespace Sitecore.MediaFramework.Pipelines.Buckets.DialogSearchFilters
{
  using Sitecore.Buckets.Pipelines.UI.DialogSearchFilters;
  using Sitecore.ContentSearch.Utilities;
  using Sitecore.Diagnostics;

  public class MediaFrameworkSearchFilters : DialogSearchFiltersProcessor
  {
    public override void Process(DialogSearchFiltersArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.SearchFilters, "args.SearchFilters");

      string filter = this.GetFilter();

      if (!args.SearchFilters.ContainsKey("MediaFramework.EmbedMedia"))
      {
        args.SearchFilters.Add("MediaFramework.EmbedMedia", filter);
      }

      if (!args.SearchFilters.ContainsKey("MediaFramework.EmbedLink"))
      {
        args.SearchFilters.Add("MediaFramework.EmbedLink", filter);
      }
    }

    protected virtual string GetFilter()
    {
      return this.GetLocationFilter();// +"&" + this.GetTemplatesFilter();
    }

    protected virtual string GetLocationFilter()
    {
      return string.Format("+location:Media Framework|{0}",IdHelper.NormalizeGuid(ItemIDs.MediaFrameworkRoot));
    }

    //protected virtual string GetTemplatesFilter()
    //{
    //  return string.Format("_templates:Media Element|{0}",IdHelper.NormalizeGuid(TemplateIDs.MediaElement));
    //}
  }
}