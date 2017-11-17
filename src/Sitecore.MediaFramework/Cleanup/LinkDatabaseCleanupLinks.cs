namespace Sitecore.MediaFramework.Cleanup
{
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Links;
  using Sitecore.MediaFramework.Diagnostics;
  using Sitecore.SecurityModel;

  public class LinkDatabaseCleanupLinks : ICleanupLinksExecuter
  {
    public virtual void CleanupLinks(Item item)
    {
      var itemLinks = Sitecore.Globals.LinkDatabase.GetReferrers(item);
      foreach (ItemLink itemLink in itemLinks)
      {
        this.RemoveLinks(itemLink);
      }
    }

    protected virtual void RemoveLinks(ItemLink itemLink)
    {
      if (itemLink.SourceFieldID != Sitecore.FieldIDs.Source)
      {
        Item sourceItem = itemLink.GetSourceItem();

        if (sourceItem != null)
        {
          LogHelper.Debug(string.Format("Removing link(TargetItemID:{0};SourceItemID:{1},SourceFieldID:{2})", itemLink.TargetItemID, itemLink.SourceItemID, itemLink.SourceFieldID), this);
            
          foreach (Item item in sourceItem.Versions.GetVersions(true))
          {
            this.RemoveLink(item, itemLink);
          }
        }
      }
    }

    protected virtual void RemoveLink(Item item, ItemLink itemLink)
    {
      Assert.ArgumentNotNull(item, "item");
      Assert.ArgumentNotNull(itemLink, "itemLink");

      Field field = item.Fields[itemLink.SourceFieldID];

      CustomField customField = FieldTypeManager.GetField(field);

      if (customField != null)
      {
        using (new SecurityDisabler())
        {
          item.Editing.BeginEdit();
          customField.RemoveLink(itemLink);
          item.Editing.EndEdit();
        }
      }
    }
  }
}