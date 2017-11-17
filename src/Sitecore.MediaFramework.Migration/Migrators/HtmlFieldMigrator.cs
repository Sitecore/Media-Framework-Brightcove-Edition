namespace Sitecore.MediaFramework.Migration.Migrators
{
  using HtmlAgilityPack;

  using Sitecore.Data.Fields;
  using Sitecore.Diagnostics;

  public abstract class HtmlFieldMigrator : NodeFieldMigrator<HtmlDocument, HtmlNode>
  {
    protected override HtmlDocument GetDocument(Field field)
    {
      Assert.ArgumentNotNull(field, "field");

      var document = new HtmlDocument();

      document.LoadHtml(field.Value);

      return document;
    }

    protected override string GetValue(HtmlDocument document)
    {
      Assert.ArgumentNotNull(document, "document");

      return document.DocumentNode.InnerHtml;
    }
  }
}