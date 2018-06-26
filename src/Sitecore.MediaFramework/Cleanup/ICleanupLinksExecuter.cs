namespace Sitecore.MediaFramework.Cleanup
{
  using Sitecore.Data.Items;

  public interface ICleanupLinksExecuter
  {
    void CleanupLinks(Item item);
  }

  //[Obsolete("Use ICleanupExecuter interface instead of this", false)]
  //public interface ICleanupLinks : ICleanupLinksExecuter
  //{
  //}
}
