namespace Sitecore.MediaFramework.Migration.Version11
{
  using System.Collections.Generic;

  using HtmlAgilityPack;

  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Diagnostics;
  using Sitecore.MediaFramework.Migration.Common;
  using Sitecore.MediaFramework.Migration.Migrators;
  using Sitecore.Text;

  public class EmbedHtmlMigrator : RichTextFieldMigrator
  {
    protected readonly IMigrationHelper Helper;

    public EmbedHtmlMigrator(IMigrationHelper helper)
    {
      this.Helper = helper;
    }

    protected override IEnumerable<HtmlNode> GetNodes(HtmlDocument document)
    {
      Assert.ArgumentNotNull(document, "document");

      return document.DocumentNode.SelectNodes("//iframe[starts-with(translate(@src, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + Constants.PlayerIframeUrl.ToLowerInvariant() + "')]");
    }

    protected virtual HtmlAttribute GetSrcAttribute(HtmlNode node)
    {
      Assert.ArgumentNotNull(node, "node");

      return node.Attributes["src"];
    }

    protected override bool MigrateNode(Field field, HtmlNode node)
    {
      Assert.ArgumentNotNull(field, "field");
      Assert.ArgumentNotNull(node, "node");

      HtmlAttribute srcAttr = this.GetSrcAttribute(node);

      if (srcAttr == null)
      {
        LogHelper.Info(string.Format("{0}({1}) '{2}' field. Source attribute could not be found.", field.Item.Name, field.Item.ID, field.Name), this);
        return false;
      }

      UrlString url = new UrlString(srcAttr.Value);

      Item mediaItem = this.Helper.GetMediaItem(field.Database, url.Parameters);
      if (mediaItem == null)
      {
        return false;
      }

      Item playerItem = this.Helper.GetPlayerItem(field.Database, url.Parameters);
      if (playerItem == null)
      {
        return false;
      }

      bool needUpdate = false;

      Item newMediaItem = this.Helper.RecreateItem(mediaItem, false);
      if (newMediaItem != null && newMediaItem.ID != mediaItem.ID)
      {
        needUpdate = true;
        url[Constants.PlayerParameters.ItemId] = newMediaItem.ID.ToShortID().ToString();
      }

      Item newPlayerItem = this.Helper.RecreateItem(playerItem, false);
      if (newPlayerItem != null && newPlayerItem.ID != playerItem.ID)
      {
        needUpdate = true;
        url[Constants.PlayerParameters.PlayerId] = newPlayerItem.ID.ToShortID().ToString();
      }

      if (needUpdate)
      {
        srcAttr.Value = url.ToString();
      }

      return needUpdate;
    }
  }
}