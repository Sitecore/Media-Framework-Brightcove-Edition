namespace Sitecore.MediaFramework.Utils
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Integration.Common.Utils;
  using Sitecore.MediaFramework.Entities;

  public static class IdUtil
  {
    public static ID GenerateItemId(Item accountItem, MediaServiceEntityData mediaData)
    {
      string key = string.Format("MediaFramework_{0}_{1}", accountItem.TemplateID, mediaData.EntityId);

      return new ID(GuidUtil.Create(GuidUtil.UrlNamespace, key));
    }
  }
}