namespace Sitecore.MediaFramework.Test.Migration.Migrators
{
  using System.Collections.Specialized;

  using Moq;

  using NUnit.Framework;

  using Sitecore.Data;
  using Sitecore.MediaFramework.Migration.Common;
  using Sitecore.TestKit.Data.Items;
  using Sitecore.TestKit.Data.Template;
  using Sitecore.TestKit.Data.Tree;

  [TestFixture]
  public class EmbedMigratorTest
  {
    protected ID TemplateId;

    protected ID ItemId;

    protected TTree Tree;

    [TestFixtureSetUp]
    public void SetUp()
    {
      this.TemplateId = ID.NewID;
      this.ItemId = new ID("{F8530460-A50F-44A6-B352-F548C8313544}");

      this.Tree = new TTree("master")
        {
          new TTemplate(this.TemplateId)
            {
              new TSection("Data")
                {
                  { "Field1", ID.NewID }, 
                }
            },
          new TItem(this.ItemId, new TemplateID(this.TemplateId))
        };
    }

    [Test]
    public void GetMediaItemShouldNotReturnsErrorWithoutId()
    {
      var mock = new Mock<MigrationHelper> { CallBase = true };

      mock.Object.GetMediaItem(this.Tree.Database, new NameValueCollection());
    }

    [TestCase("", false)]
    [TestCase("F8530460A50F44A6B352F548C8313544", true)]
    [TestCase("f8530460A50F44A6B352F548C8313544", true)]
    [TestCase("A95F37CDE2A948D5AA8970F8567D332F", false)]
    [TestCase("{F8530460-A50F-44A6-B352-F548C8313544}",false)]
    public void GetMediaItemShouldReturnsItem(string itemId, bool correctId)
    {
      var mock = new Mock<MigrationHelper> { CallBase = true };

      var collection = new NameValueCollection();
      collection[Constants.PlayerParameters.ItemId] = itemId;

      var mediaItem = mock.Object.GetMediaItem(this.Tree.Database, collection);

      if (correctId)
      {
        Assert.NotNull(mediaItem);
      }
      else
      {
        Assert.IsNull(mediaItem);
      }
    }

    [Test]
    public void GetPlayerItemShouldNotReturnsErrorWithoutId()
    {
      var mock = new Mock<MigrationHelper> { CallBase = true };

      mock.Object.GetPlayerItem(this.Tree.Database, new NameValueCollection());
    }

    [TestCase("", false)]
    [TestCase("F8530460A50F44A6B352F548C8313544", true)]
    [TestCase("f8530460A50F44A6B352F548C8313544", true)]
    [TestCase("A95F37CDE2A948D5AA8970F8567D332F", false)]
    [TestCase("{F8530460-A50F-44A6-B352-F548C8313544}", false)]
    public void GetPlayerItemShouldReturnsItem(string itemId, bool correctId)
    {
      var mock = new Mock<MigrationHelper> { CallBase = true };

      var collection = new NameValueCollection();
      collection[Constants.PlayerParameters.PlayerId] = itemId;

      var mediaItem = mock.Object.GetPlayerItem(this.Tree.Database, collection);

      if (correctId)
      {
        Assert.NotNull(mediaItem);
      }
      else
      {
        Assert.IsNull(mediaItem);
      }
    }

    [TestFixtureTearDown]
    public void TearDown()
    {
      this.Tree.Dispose();
    }
  }
}