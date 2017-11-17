namespace Sitecore.MediaFramework.Synchronize.Fallback
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Entities;

  public abstract class DatabaseFallbackBase : IDatabaseFallback
  {
    public virtual MediaServiceSearchResult Fallback(object entity, Item accountItem)
    {
      Item item = this.GetItem(entity, accountItem);

      if (item != null)
      {
        var searchResult = this.GetSearchResult(item);

        this.FillStandardProperties(searchResult, item);

        return searchResult;
      }
      return null;
    }

    protected abstract Item GetItem(object entity, Item accountItem);
    protected abstract MediaServiceSearchResult GetSearchResult(Item item);

    protected virtual void FillStandardProperties(MediaServiceSearchResult searchResult, Item item)
    {
      searchResult.Uri = new ItemUri(item);

      searchResult.Name = item.Name;
      searchResult.DatabaseName = searchResult.Uri.DatabaseName;
      searchResult.Language = searchResult.Uri.Language.Name;
      searchResult.ItemId = searchResult.Uri.ItemID;
      searchResult.Version = searchResult.Uri.Version.ToString();
    }
  }
}