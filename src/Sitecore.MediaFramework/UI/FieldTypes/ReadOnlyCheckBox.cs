// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyCheckBox.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   Read-Only CheckBox
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.UI.FieldTypes
{
  using System;

  using Sitecore.Shell.Applications.ContentEditor;

  /// <summary>
  /// Read-Only CheckBox
  /// </summary>
  public class ReadOnlyCheckBox : Sitecore.Web.UI.HtmlControls.Checkbox, IContentField
  {
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
      this.Disabled = true;
      //this.Background = "Transparent";
      base.OnPreRender(e);
    }
  }
}