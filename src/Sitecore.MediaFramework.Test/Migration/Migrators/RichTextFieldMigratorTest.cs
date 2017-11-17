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
  public class RichTextFieldMigratorTest
  {
    [Test]
    public void GetFieldsShouldReturnsOnlyRteFields()
    {
      var templateId = ID.NewID;
      var itemId = ID.NewID;

      using (
        var tree = new TTree("master")
          {
            new TTemplate(templateId)
              {
                new TSection("Data")
                  {
                    {"Field1",ID.NewID,"rich text"},
                    {"Field2",ID.NewID,"Rich text"},
                    {"Field3",ID.NewID},
                  }
              },
            new TItem(itemId, new TemplateID(templateId))
          })
      {
        var item = tree.Database.GetItem(itemId);
        
        item.Fields.ReadAll();

        var mock = new Mock<RichTextFieldMigrator> { CallBase = true };

        mock.Object.MigrateItem(item);

        mock.Verify(i => i.MigrateField(It.IsAny<Field>()), Times.Exactly(2));
      }
    }
  }
}