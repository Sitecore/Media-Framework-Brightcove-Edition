namespace Sitecore.MediaFramework.Test.Migration.Migrators
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml;

  using Moq;
  using Moq.Protected;

  using NUnit.Framework;

  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.MediaFramework.Migration.Migrators;
  using Sitecore.TestKit.Data.Items;
  using Sitecore.TestKit.Data.Template;
  using Sitecore.TestKit.Data.Tree;

  [TestFixture]
  public class NodeFieldMigratorTest
  {
    [TestCase(1)]
    [TestCase(5)]
    [TestCase(0)]
    [TestCase(-1)]
    public void MigrateNodeShouldBeCalledForEachNode(int count)
    {
      var templateId = ID.NewID;
      var itemId = ID.NewID;

      var nodes = count > 0 ? Enumerable.Repeat(It.IsAny<XmlDocument>(), count).ToList() : null;
      
      using (
        var tree = new TTree("master")
          {
            new TTemplate(templateId)
              {
                new TSection("Data") { {"Field1",ID.NewID}}
              },
            new TItem(itemId, new TemplateID(templateId))
          })
      {
        var item = tree.Database.GetItem(itemId);

        var mock = new Mock<NodeFieldMigrator<XmlDocument, XmlNode>> { CallBase = true };

        mock.Protected()
            .Setup<IEnumerable<XmlNode>>("GetNodes", new object[] { ItExpr.IsAny<XmlDocument>() })
            .Returns(
              nodes);

        
        mock.Object.MigrateField(item.Fields["Field1"]);

        mock.Protected().Verify(
          "MigrateNode", 
          Times.Exactly(nodes != null ? nodes.Count : 0), 
          new object[] { ItExpr.IsAny<Field>(), ItExpr.IsAny<XmlNode>() });
      }
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateFieldShouldbeCalled(bool needUpdate)
    {
      var templateId = ID.NewID;
      var itemId = ID.NewID;

      using (
        var tree = new TTree("master")
          {
            new TTemplate(templateId)
              {
                new TSection("Data") { {"Field1",ID.NewID}}
              },
            new TItem(itemId, new TemplateID(templateId))
          })
      {
        var item = tree.Database.GetItem(itemId);

        var mock = new Mock<NodeFieldMigrator<XmlDocument, XmlNode>> { CallBase = true };

        mock.Protected()
            .Setup<IEnumerable<XmlNode>>("GetNodes", new object[] { ItExpr.IsAny<XmlDocument>() })
            .Returns(new[] { It.IsAny<XmlDocument>(), It.IsAny<XmlDocument>() });

        mock.Protected()
            .Setup<bool>("MigrateNode", new object[] { ItExpr.IsAny<Field>(), ItExpr.IsAny<XmlNode>() })
            .Returns(needUpdate);

        mock.Object.MigrateField(item.Fields["Field1"]);

        mock.Protected().Verify(
          "UpdateField",
          needUpdate ? Times.Once() : Times.Never(),
          new object[] { ItExpr.IsAny<Field>(), ItExpr.IsAny<XmlDocument>() });
      }
    }
  }
}