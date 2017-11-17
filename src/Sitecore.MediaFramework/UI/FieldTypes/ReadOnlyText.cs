// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyText.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   Present ReadOnly field type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.UI.FieldTypes
{
  using System;

  using Sitecore.Shell.Applications.ContentEditor;
  using Sitecore.Web.UI.HtmlControls;

  /// <summary> 
  /// Present ReadOnly field type. 
  /// </summary>
  public class ReadOnlyText : Edit, IContentField
  {
    public ReadOnlyText()
    {
      base.Class = "scContentControl";
      base.Activation = true;
    }

    public string GetValue()
    {
      return this.Value;
    }

    public void SetValue(string value)
    {
      this.Value = value;
    }

    protected override void OnPreRender(EventArgs e)
    {
      this.ReadOnly = true;
      this.Background = "Transparent";
      base.OnPreRender(e);
    }
  }
}