namespace Sitecore.MediaFramework.Migration.Version11
{
  using System.Collections.Generic;

  using HtmlAgilityPack;

  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Migration.Common;

  public class EmbedLinkMigrator : EmbedHtmlMigrator
  {
    public EmbedLinkMigrator(IMigrationHelper helper) : base(helper)
    {
    }

    protected override IEnumerable<HtmlNode> GetNodes(HtmlDocument document)
    {
      Assert.ArgumentNotNull(document, "document");

      return document.DocumentNode.SelectNodes("//a[starts-with(translate(@href, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + Constants.PlayerIframeUrl.ToLowerInvariant() + "')]");
    }

    protected override HtmlAttribute GetSrcAttribute(HtmlNode node)
    {
      Assert.ArgumentNotNull(node, "node");

      return node.Attributes["href"];
    }
  }
}