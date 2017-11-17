namespace Sitecore.MediaFramework.Migration.Migrators
{
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Web;
  using System.Xml;

  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Diagnostics;

  public abstract class RenderingsFieldMigrator : XmlFieldMigrator
  {
    protected override IEnumerable<Field> GetFields(Item item)
    {
      Assert.ArgumentNotNull(item, "item");

      Field field = item.Fields[Sitecore.FieldIDs.LayoutField];
      if (field != null && field.HasValue)
      {
        yield return field;
      }
    }

    protected virtual Item GetDatasourceItem(Field field, XmlNode node)
    {
      Assert.ArgumentNotNull(field,"field");
      Assert.ArgumentNotNull(node,"node");

      string datasource = this.GetDatasource(node);

      Item item = null;

      if (!string.IsNullOrEmpty(datasource))
      {
        item = field.Database.GetItem(datasource);
      }

      if (item == null)
      {
        LogHelper.Info("Media item could not be found. Datasource:" + datasource, this);
      }
      return item;
    }

    protected virtual string GetId(XmlNode node)
    {
      XmlAttribute attribute = this.GetRenderingAttribute(node, "id");

      return attribute != null ? attribute.Value : null;
    }

    protected virtual string GetDatasource(XmlNode node)
    {
      XmlAttribute attribute = this.GetRenderingAttribute(node, "ds");

      return attribute != null ? attribute.Value : null;
    }

    protected virtual bool SetDatasource(XmlNode node, ID datasource)
    {
      XmlAttribute attribute = this.GetRenderingAttribute(node, "ds");

      if (attribute != null)
      {
        attribute.Value = datasource.ToString();
        return true;
      }

      return false;
    }

    protected virtual NameValueCollection GetParameters(XmlNode node)
    {
      XmlAttribute attribute = this.GetRenderingAttribute(node, "par");

      return attribute != null && !string.IsNullOrEmpty(attribute.Value) ? HttpUtility.ParseQueryString(attribute.Value) : new NameValueCollection();
    }

    protected virtual bool SetParameters(XmlNode node,NameValueCollection parameters)
    {
      XmlAttribute attribute = this.GetRenderingAttribute(node, "par");

      if (attribute != null)
      {
        attribute.Value = StringUtil.NameValuesToString(parameters, "&");
        return true;
      }

      return false;
    }

    protected virtual XmlAttribute GetRenderingAttribute(XmlNode node, string name)
    {
      Assert.ArgumentNotNull(node, "node");
      
      if (node.Attributes != null)
      {
        return node.Attributes["s:" + name] ?? node.Attributes["p:" + name] ?? node.Attributes[name];
      }

      return null;
    }
  }
}