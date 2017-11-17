namespace Sitecore.MediaFramework.Cleanup
{
  using System.Collections.Generic;

  using Sitecore.Data.Items;

  public interface ICleanupExecuter
  {
    IEnumerable<Item> GetScopeItems(Item accountItem);
  }
}