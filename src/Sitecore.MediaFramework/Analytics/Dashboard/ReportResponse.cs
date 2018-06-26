// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportResponse.cs" company="Sitecore A/S">
//   Copyright (C) 2014 by Sitecore A/S
// </copyright>
// <summary>
//   Defines the ReportResponse type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Analytics.Dashboard
{
  using System.Collections.Generic;
  using System.Linq;
  using Newtonsoft.Json;
  using Sitecore.ExperienceAnalytics.Api.Response;

  public class ReportResponse
  {
    private readonly IList<Message> messages = new List<Message>();

    public ReportResponse()
    {
      this.data = new ReportData();
    }

    public ReportData data { get; set; }

    public Message[] Messages
    {
      get
      {
        return this.messages.ToArray();
      }
    }

    public int TotalRecordCount { get; set; }


    public void AddMessage(Message message)
    {
      this.messages.Add(message);
    }
  }
}