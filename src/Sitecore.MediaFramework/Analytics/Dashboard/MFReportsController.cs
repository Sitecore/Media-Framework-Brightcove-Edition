// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportsController.cs" company="Sitecore A/S">
//   Copyright (C) 2014 by Sitecore A/S
// </copyright>
// <summary>
//   Defines the ReportsController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Analytics.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Results;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.ExperienceAnalytics.Api;
    using Sitecore.ExperienceAnalytics.Api.Encoding;
    using Sitecore.Services.Infrastructure.Web.Http;
    using Xdb.Reporting;

    public class MFReportsController : ServicesApiController
  {
    [HttpGet]
    public IHttpActionResult Get(string datasource, string siteName)
    {
      ReportDataProviderBase reportingDataProvider = ApiContainer.Configuration.GetReportingDataProvider();
            var cachingPolicy = new CachingPolicy
            {
                ExpirationPeriod = TimeSpan.FromHours(1)
        //must find cache expiration node
      };

      Item dataSourceItem = Database.GetDatabase("core").GetItem(new ID(datasource));
      var reportSQLQuery = dataSourceItem.Fields["{0AA8B742-BBDF-4405-AB8D-6FAC7E79433B}"].Value;

      NameValueCollection parameters = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);
      var from = DateTime.ParseExact(parameters["dateFrom"], "dd-MM-yyyy", new DateTimeFormatInfo());
      var to = DateTime.ParseExact(parameters["dateTo"], "dd-MM-yyyy", new DateTimeFormatInfo());
      string dateFrom = from.ToString("yyyy-MM-dd");
      string dateTo = to.ToString("yyyy-MM-dd");
      if (from.Equals(to) && parameters["dateTo"].Length <= 10)
      {
        dateFrom = from.ToString("yyyy-MM-dd 00:00:00");
        dateTo = to.ToString("yyyy-MM-dd 23:59:59");
      }
      
      reportSQLQuery = reportSQLQuery.Replace("@StartDate", "'" + dateFrom + "'");
      reportSQLQuery = reportSQLQuery.Replace("@EndDate", "'" + dateTo + "'");

      string hashedSiteName = "0";
      if (siteName != "all")
      {
        var encoder = new Hash32Encoder();
        hashedSiteName = encoder.Encode(siteName);
        reportSQLQuery = reportSQLQuery.Replace("@SiteNameIdOperator", "=");
      }
      else
      {
        reportSQLQuery = reportSQLQuery.Replace("@SiteNameIdOperator", "!=");
      }

      reportSQLQuery = reportSQLQuery.Replace("@SiteNameId", hashedSiteName);

      var query = new ReportDataQuery(reportSQLQuery);

      DataTableReader reader = reportingDataProvider.GetData("reporting", query, cachingPolicy).GetDataTable().CreateDataReader();
      
      var data = new ReportData();

      int counter = 0;
      while (reader.Read())
      {
        var row = new Dictionary<string, string>();
        for (int i = 0; i < reader.FieldCount; i++)
        {
          row.Add(reader.GetName(i), reader[i].ToString());
        }

        data.AddRow(row);
        counter++;
      }

      var responce = new ReportResponse
                       {
                         data = data,
                         TotalRecordCount = counter
                       };

      return new JsonResult<ReportResponse>(responce, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }, Encoding.UTF8, this);
    }
  }
}