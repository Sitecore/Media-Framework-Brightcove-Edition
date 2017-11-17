namespace Sitecore.MediaFramework.Mvc.Data
{
  using System.Collections.Generic;
  using System.Text;
  using System.Web;

  using Sitecore.Diagnostics;

  public class HtmlResources
  {
    protected const string CssLinkTemplate = "<link rel='stylesheet' type='text/css' href='{0}'/>";
    protected const string ScriptLinkTemplate = "<script type='text/javascript' src='{0}'></script>";

    public ISet<string> CssUrls { get; set; }
    public ISet<string> ScriptUrls { get; set; }
    public IList<string> HtmlBlocks { get; set; }

    public HtmlResources()
    {
      this.CssUrls = new HashSet<string>();
      this.ScriptUrls = new HashSet<string>();
      this.HtmlBlocks = new List<string>();
    }

    public void AddCssUrl(string url)
    {
      this.CssUrls.Add(url);
    }

    public void AddScriptUrl(string url)
    {
      this.ScriptUrls.Add(url);
    }

    public void AddHtmlBlock(string url)
    {
      this.HtmlBlocks.Add(url);
    }

    public virtual void ExceptWith(HtmlResources resources)
    {
      Assert.ArgumentNotNull(resources, "resources");

      this.CssUrls.ExceptWith(resources.CssUrls);
      this.ScriptUrls.ExceptWith(resources.ScriptUrls);
    }

    public virtual void UnionWith(HtmlResources resources)
    {
      Assert.ArgumentNotNull(resources, "resources");

      this.CssUrls.UnionWith(resources.CssUrls);
      this.ScriptUrls.UnionWith(resources.ScriptUrls);
      
      foreach (string htmlBlock in resources.HtmlBlocks)
      {
        this.HtmlBlocks.Add(htmlBlock);
      }
    }

    public virtual IHtmlString Render()
    {
      var builder = new StringBuilder();
      foreach (var path in this.CssUrls)
      {
        builder.AppendLine(string.Format(CssLinkTemplate, path));
      }
      foreach (var path in this.ScriptUrls)
      {
        builder.AppendLine(string.Format(ScriptLinkTemplate, path));
      }
      foreach (var htmlBlock in this.HtmlBlocks)
      {
        builder.AppendLine(htmlBlock);
      }
      return new HtmlString(builder.ToString());
    }
  }
}