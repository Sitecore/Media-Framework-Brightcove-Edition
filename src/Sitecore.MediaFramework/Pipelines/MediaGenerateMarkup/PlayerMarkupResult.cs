namespace Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup
{
  using System.Collections.Generic;

  public class PlayerMarkupResult
  {
    public PlayerMarkupResult()
    {
      this.Html = string.Empty;
      this.CssUrls = new List<string>();
      this.ScriptUrls = new List<string>();
      this.BottomScripts = new Dictionary<string, string>();
    }

    public string Html { get; set; }
    public List<string> CssUrls { get;private set; }
    public List<string> ScriptUrls { get; private set; }

    public Dictionary<string, string> BottomScripts { get; private set; } 
  }
}