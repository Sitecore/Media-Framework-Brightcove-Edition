
namespace Sitecore.MediaFramework.Test.Migration
{
  using System.Linq;
  using System.Xml;

  using Moq;
  using Moq.Protected;

  using NUnit.Framework;

  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Migration;
  using Sitecore.MediaFramework.Migration.Migrators;
  using Sitecore.TestKit.Data.Items;
  using Sitecore.TestKit.Data.Tree;

  [TestFixture]
  public class MigrationProviderTest
  {
    protected ID RootId = ID.NewID;
    protected TTree Tree;

    [TestFixtureSetUp]
    public void SetUp()
    {
      this.Tree = new TTree("master")
        {
            new TItem("account", this.RootId, ID.NewID)
              {
                new TItem("Media Content",new TemplateID(Sitecore.TemplateIDs.Folder))
                  {
                    new TItem("Media Content 1"),
                    new TItem("Media Content 2"),
                    new TItem("Media Content 3"),
                  },
                new TItem("Players",new TemplateID(Sitecore.TemplateIDs.Folder))
                  {
                    new TItem("Players 1"),
                    new TItem("Players 2"),
                    new TItem("Players 3"),
                  },
                new TItem("Etc",new TemplateID(Sitecore.TemplateIDs.Folder))
                  {
                    new TItem("Etc 1"),
                    new TItem("Etc 2"),
                    new TItem("Etc 3"),
                  },
              }
        };
    }

    [TestCase("<add type=\"Sitecore.MediaFramework.Migration.Version11.EmbedHtmlMigrator, Sitecore.MediaFramework.Migration\">"
              + "<param desc=\"helper\" type=\"Sitecore.MediaFramework.Migration.Common.MigrationHelper, Sitecore.MediaFramework.Migration\"/></add>",1)]
    [TestCase("<add type=\"System.String\" />", 0)]
    public void AddMigratorShouldWorks(string xml, int count)
    {
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(xml);

      var mock = new Mock<MigrationProvider> { CallBase = true };

      mock.Object.AddMigrator(doc.DocumentElement);

      Assert.AreEqual(count, mock.Object.Migrators.Count);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void GetItemsShouldReturnsDescedants(bool isDeep)
    {
      Item item = this.Tree.Database.GetItem(this.RootId);

      var mock = new Mock<MigrationProvider> { CallBase = true };
      var items = mock.Object.GetItems(item, isDeep);

      int count = isDeep ? item.Axes.GetDescendants().Length + 1 : 1;

      Assert.AreEqual(count, items.Count());
    }

    [Test]
    public void MigrateContentShouldCallMigrateItem()
    {
      Item item = this.Tree.Database.GetItem(this.RootId);

      var mock = new Mock<MigrationProvider> { CallBase = true };

      mock.Setup(i => i.GetItems(It.IsAny<Item>(), It.IsAny<bool>())).Returns(new []{item}.Union(item.Axes.GetDescendants()));

      mock.Protected().Setup("BeforeContentMigration", ItExpr.IsAny<Item>(), ItExpr.IsAny<bool>());

      mock.Object.MigrateContent(item, true);

      mock.Verify(i => i.MigrateItem(It.IsAny<Item>()), Times.Exactly(item.Axes.GetDescendants().Length + 1));
    }

    [Test]
    public void MigrateContentShouldCallUpdateJobStatus()
    {
      Item item = this.Tree.Database.GetItem(this.RootId);

      var mock = new Mock<MigrationProvider> { CallBase = true };

      mock.Setup(i => i.GetItems(It.IsAny<Item>(), It.IsAny<bool>())).Returns(new[] { item }.Union(item.Axes.GetDescendants()));

      mock.Protected().Setup("BeforeContentMigration", ItExpr.IsAny<Item>(), ItExpr.IsAny<bool>());

      mock.Object.MigrateContent(item, true);

      mock.Protected().Verify("UpdateJobStatus", Times.Exactly(item.Axes.GetDescendants().Length + 1), ItExpr.IsAny<Item>(), ItExpr.IsAny<string>());
    }

    [Test]
    public void EachMigratorShouldBeCalled()
    {
      Item item = this.Tree.Database.GetItem(this.RootId);

      var mock = new Mock<MigrationProvider> { CallBase = true };
      var migrator1 = new Mock<FieldMigrator> { CallBase = true };
      var migrator2 = new Mock<FieldMigrator> { CallBase = true };

      mock.Object.Migrators.Add(migrator1.Object);
      mock.Object.Migrators.Add(migrator2.Object);

      mock.Setup(i => i.GetItems(It.IsAny<Item>(), It.IsAny<bool>())).Returns(new[] { item });

      mock.Object.MigrateItem(item);
      migrator1.Verify(i => i.MigrateItem(It.IsAny<Item>()), Times.Once());
      migrator2.Verify(i => i.MigrateItem(It.IsAny<Item>()), Times.Once());
    }

    [TestFixtureTearDown]
    public void TearDown()
    {
      this.Tree.Dispose();
    }
  }
}
