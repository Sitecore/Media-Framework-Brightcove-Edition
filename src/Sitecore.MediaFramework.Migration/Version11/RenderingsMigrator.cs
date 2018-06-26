namespace Sitecore.MediaFramework.Migration.Version11
{
  using System.Collections.Generic;
  using System.Xml;

  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Migration.Common;
  using Sitecore.MediaFramework.Migration.Migrators;

  public class RenderingsMigrator : RenderingsFieldMigrator
  {
    protected readonly IMigrationHelper Helper;

    public RenderingsMigrator(IMigrationHelper helper)
    {
      this.Helper = helper;
    }

    protected override IEnumerable<XmlNode> GetNodes(XmlDocument document)
    {
      Assert.ArgumentNotNull(document, "document");

      XmlNodeList xmlNodeList = document.SelectNodes("//r");
      if (xmlNodeList == null)
      {
        yield break;
      }

      string sublayoutId = ItemIDs.EmbedMediaSublayout.ToString();

      foreach (object obj in xmlNodeList)
      {
        XmlNode node = obj as XmlNode;

        if (node != null)
        {
          string renderingId = this.GetId(node);

          if (!string.IsNullOrEmpty(renderingId) && renderingId == sublayoutId)
          {
            yield return node;
          }
        }
      }
    }

    protected override bool MigrateNode(Field field, XmlNode node)
    {
      Assert.ArgumentNotNull(field, "field");
      Assert.ArgumentNotNull(node, "node");

      if (node.Attributes == null)
      {
        return false;
      }

      Item mediaItem = this.GetDatasourceItem(field, node);
      if (mediaItem == null)
      {
        return false;
      }

      var parameters = this.GetParameters(node);

      Item playerItem = this.Helper.GetPlayerItem(field.Database, parameters);
      if (playerItem == null)
      {
        return false;
      }

      bool needUpdate = false;

      Item newMediaItem = this.Helper.RecreateItem(mediaItem, false);
      if (newMediaItem != null && newMediaItem.ID != mediaItem.ID)
      {
        needUpdate = true;
        this.SetDatasource(node, newMediaItem.ID);
      }

      Item newPlayerItem = this.Helper.RecreateItem(playerItem, false);
      if (newPlayerItem != null && newPlayerItem.ID != playerItem.ID)
      {
        needUpdate = true;
        parameters[Constants.PlayerParameters.PlayerId] = newPlayerItem.ID.ToShortID().ToString();

        this.SetParameters(node, parameters);
      }

      return needUpdate;
    }
  }
}