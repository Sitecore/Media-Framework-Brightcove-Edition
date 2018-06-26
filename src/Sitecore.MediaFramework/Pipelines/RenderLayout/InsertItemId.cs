namespace Sitecore.MediaFramework.Pipelines.RenderLayout
{
  using System.Web.UI;
  using System.Web.UI.WebControls;

  using Sitecore.Data.Items;

  public class InsertItemId : RenderLayoutBase
  {
    protected override Control GetControlToRender()
    {
      Item item = Context.Item;
      if (item != null)
      {
        return new HiddenField { ID = "MediaFramework_ItemId", ClientIDMode = ClientIDMode.Static, Value = item.ID.ToShortID().ToString() };
      }

      return null;
    }
  }
}