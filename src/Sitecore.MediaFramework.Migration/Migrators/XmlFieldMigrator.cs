namespace Sitecore.MediaFramework.Migration.Migrators
{
  using System.Xml;

  using Sitecore.Data.Fields;
  using Sitecore.Diagnostics;

  public abstract class XmlFieldMigrator : NodeFieldMigrator<XmlDocument, XmlNode>
  {
    protected override XmlDocument GetDocument(Field field)
    {
      Assert.ArgumentNotNull(field, "field");

      XmlDocument document = new XmlDocument();

      document.LoadXml(field.Value);

      return document;
    }

    protected override string GetValue(XmlDocument document)
    {
      Assert.ArgumentNotNull(document, "document");

      return document.OuterXml;
    }
  }
}