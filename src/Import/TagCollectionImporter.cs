namespace Sitecore.MediaFramework.Brightcove.Import
{
  using System.Collections.Generic;
  using System.Linq;

  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Brightcove.Entities;

  public class TagCollectionImporter : EntityCollectionImporter<Video>
  {
    public override IEnumerable<object> GetData(Item accountItem)
    {
      var data = base.GetData(accountItem);

      return this.ReadAllTags(data.OfType<Video>());
    }

    protected virtual IEnumerable<Tag> ReadAllTags(IEnumerable<Video> videoList)
    {
      foreach (Video video in videoList)
      {
        if (video.Tags == null || video.Tags.Count == 0)
        {
          continue;
        }

        foreach (string tag in video.Tags)
        {
          yield return new Tag { Name = tag };
        }
      }
    }

    protected override string RequestName
    {
      get
      {
        return "read_tags";
      }
    }
  }
}