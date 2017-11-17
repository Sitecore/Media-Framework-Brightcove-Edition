// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportData.cs" company="Sitecore A/S">
//   Copyright (C) 2014 by Sitecore A/S
// </copyright>
// <summary>
//   Defines the ReportData type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Analytics.Dashboard
{
  using System;
  using System.Collections.Generic;
  using Newtonsoft.Json;
  using Newtonsoft.Json.Serialization;
  using Sitecore.ExperienceAnalytics.Api.Response;

  public class ReportData
  {
    public ReportData()
    {
      this.Localization = new Localization();
      this.Content = new List<Dictionary<string, string>>();
    }

    public List<Dictionary<string, string>> Content { get; set; }

    public Localization Localization { get; set; }


    public void AddRow(Dictionary<string, string> row)
    {
      this.Content.Add(row);
    }
  }
}