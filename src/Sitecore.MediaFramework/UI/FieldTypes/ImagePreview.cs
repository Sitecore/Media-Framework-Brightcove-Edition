namespace Sitecore.MediaFramework.UI.FieldTypes
{
  using Sitecore.Globalization;
  using Sitecore.Shell.Applications.ContentEditor;

  public class ImagePreview : Sitecore.Web.UI.HtmlControls.Control, IContentField
  {  
    public string Source { get; set; }

    public string GetValue()
    {
      return this.Value;
    }

    public void SetValue(string value)
    {
      this.Value = value;
    }

    /// <summary>
    /// Do Render
    /// </summary>
    /// <param name="output">
    /// The output.
    /// </param>
    protected override void DoRender(System.Web.UI.HtmlTextWriter output)
    {
      string value = this.Value;
      if (!string.IsNullOrEmpty(value))
      {
        var values = StringUtil.GetNameValues(this.Source, '=', '&');

        output.Write("<img width='" + values["Width"] + "' src='" + value + "'>");
      }
      else
      {
        output.Write("<div>" + Translate.Text(Translations.AnAssetDoesNotHaveAnyPreviewImage) + "</div>");
      }
    }
  }
}