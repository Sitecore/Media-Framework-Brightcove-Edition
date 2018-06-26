// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExperienceAnalyticsListControlViewModel.cs" company="Sitecore A/S">
//   Copyright (C) 2014 by Sitecore A/S
// </copyright>
// <summary>
//   Defines the ExperienceAnalyticsListControlViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Analytics.Dashboard.Layouts.Renderings
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Sitecore;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.ExperienceAnalytics.Client;
    using Sitecore.ExperienceAnalytics.Client.Mvc.Presentation;
    using Sitecore.ExperienceAnalytics.Core.Diagnostics;
    using Sitecore.Links;
    using Sitecore.Mvc.Presentation;

    using Globals = Sitecore.ExperienceAnalytics.Client.Globals;
    using System;
    using Web.UI.Controls;

    public class ExperienceAnalyticsListControlViewModel : DvcRenderingModel
  {
    // Fields
    private readonly ILogger logger;
    
    // Methods
    public ExperienceAnalyticsListControlViewModel()
    {
      base.UseTimeResolution = false;
      this.logger = ClientContainer.GetLogger();
    }


    public string ColumnsItemID { get; protected set; }

    public string DataSource { get; protected set; }


    public override void Initialize(Rendering rendering)
    {
      base.Initialize(rendering);
      base.Control.Class = "sc-ExperienceAnalyticsListControl";
      base.Control.Requires.Script("/sitecore/shell/client/Applications/ExperienceAnalytics/Common/Layouts/Renderings/MediaFramework/ExperienceAnalyticsListControl.js");
      base.Control.HasNestedComponents = true;
      int num = (base.Control.GetInt("KeysCount", 0) == 0) ? 10 : base.Control.GetInt("KeysCount", 0);
      string str = base.Control.GetString("KeysSortByMetric");
      string str2 = base.Control.GetString("KeysSortDirection");
      string str3 = string.IsNullOrEmpty(str2) ? DvcRenderingModel.GetTextValue(Globals.SortDirection.Descending) : DvcRenderingModel.GetTextValue(new ID(str2));
      string str4 = base.Control.GetString("TargetPage");
      bool flag = true;
      bool flag2 = true;

      Item dataSourceItem = Context.Database.GetItem(new ID(base.Control.DataSource));
      this.ColumnsItemID = dataSourceItem.Fields["{6B155797-3EBE-4E58-BBF1-6180FB0CDC4B}"].Value;
      this.DataSource = base.Control.DataSource;
      base.Control.Attributes["data-sc-datasource"] = base.Control.DataSource;
      
      IEnumerable<Item> items = Context.Database.GetItems(this.ColumnsItemID);
      var dictionary = new Dictionary<string, object>();
      foreach (Item item in items)
      {
        string dataFieldValue = DvcRenderingModel.GetDataFieldValue(item.ID);
        object numberScaleObject = DvcRenderingModel.GetNumberScaleObject(item.ID);

        if (numberScaleObject != null)
        {
          dictionary.Add(dataFieldValue, numberScaleObject);
        }
      }

      base.Control.Attributes.Add("data-sc-columnfields", 
        JsonConvert.SerializeObject(
          dictionary, 
          Formatting.Indented,
          new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
      base.Control.Attributes["data-sc-keygrouping"] = GetKeyGrouping(base.Control);
      base.Control.Attributes["data-sc-componenttype"] = DvcRenderingModel.GetTextValue(Globals.System.Texts.List);
      base.Control.DataBind = "visible: isVisible";

      if (!string.IsNullOrEmpty(str4))
      {
        flag = ID.IsID(str4);
        if (flag)
        {
          Item item2 = Context.Database.GetItem(new ID(str4));
          flag2 = item2 != null;
          if (flag2)
          {
            string itemUrl = LinkManager.GetItemUrl(item2);
            base.Control.Attributes.Add("data-sc-targetpageurl", itemUrl);
          }
          else
          {
            this.logger.Warn("The TargetPage does not contain a valid item id for controlid: " + base.Control.ControlId);
          }
        }
        else
        {
          this.logger.Warn("The TargetPage does not contain a valid item id for controlid: " + base.Control.ControlId);
        }
      }

      if (!string.IsNullOrEmpty(str))
      {
        base.Control.Attributes.Add("data-sc-orderkeysby", DvcRenderingModel.GetDataFieldValue(new ID(str)));
      }

      base.Control.Attributes.Add("data-sc-keysdirection", str3);
      base.Control.Attributes.Add("data-sc-keyscount", num.ToString(CultureInfo.InvariantCulture));
      var type = new
      {
        GenericServerError = DvcRenderingModel.GetTextValue(Globals.System.Texts.ErrorMessages.GenericServerError),
        GenericServerWarning = DvcRenderingModel.GetTextValue(Globals.System.Texts.ErrorMessages.GenericServerWarning),
        NotAllowedCharacters = DvcRenderingModel.GetTextValue(Globals.System.Texts.ErrorMessages.NotAllowedCharacters),
        WrongTargetPage = (!flag2 || !flag) ? DvcRenderingModel.GetTextValue(Globals.System.Texts.ErrorMessages.GenericServerWarning) : string.Empty
      };

      base.Control.Attributes.Add("data-sc-errortexts", JsonConvert.SerializeObject(type));
    }

        protected override string GetKeyGrouping(ControlBase rendering)
        {
            if (rendering == null)
                throw new ArgumentNullException("rendering");
            string path = rendering.GetString("KeyGrouping");
            Item obj1 = Context.Database.GetItem(Globals.KeyGrouping.ByKey);
            if (!string.IsNullOrEmpty(path))
            {
                Item obj2 = Context.Database.GetItem(path);
                if (obj2 != null)
                    obj1 = obj2;
            }
            return obj1["Text"];
        }
    }
}