
namespace Sitecore.MediaFramework.Test.Migration.Migrators
{
  using Moq;

  using NUnit.Framework;

  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.MediaFramework.Migration.Migrators;
  using Sitecore.TestKit.Data.Items;
  using Sitecore.TestKit.Data.Template;
  using Sitecore.TestKit.Data.Tree;

  [TestFixture]
  public class FieldMigratorTest
  {
    [Test]
    public void MigrateFieldShouldBeCalledForEachField()
    {
      var templateId = ID.NewID;
      var itemId = ID.NewID;
      using (
        var tree = new TTree("master")
          {
            new TTemplate(templateId)
              {
                new TSection("Data") { {"Field1",ID.NewID}, {"Field2",ID.NewID }, {"Field3",ID.NewID }, }
              },
            new TItem(itemId, new TemplateID(templateId))
          })
      {
        var item = tree.Database.GetItem(itemId);

        item.Fields.ReadAll();

        Assert.AreEqual(item.Fields.Count, 3);

        var mock = new Mock<FieldMigrator> { CallBase = true };

        mock.Object.MigrateItem(item);

        mock.Verify(i => i.MigrateField(It.IsAny<Field>()), Times.Exactly(item.Fields.Count));
      }
    }
  }
}
