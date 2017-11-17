namespace Sitecore.MediaFramework.Commands
{
  using System;

  using Sitecore.Data;
  using Sitecore.Diagnostics;
  using Sitecore.Layouts;
  using Sitecore.Shell.Framework.Commands;
  using Sitecore.Text;
  using Sitecore.Web;
  using Sitecore.Web.UI.Sheer;

  [Serializable]
  public class EmbedMedia : Command
  {
    public override void Execute(CommandContext context)
    {
      Context.ClientPage.Start(this, "Run", context.Parameters);
    }

    /// <summary>
    /// Run Form
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    protected void Run(ClientPipelineArgs args)
    {
      if (!args.IsPostBack)
      {
        var rendering = GetRendering(ShortID.Decode(args.Parameters["uniqueid"]));
        if (rendering == null)
        {
          return;
        }
        var urlString = new UrlString(UIUtil.GetUri("control:MediaFramework.EmbedMedia"));
        urlString["mo"] = "webedit";

        if (!string.IsNullOrEmpty(rendering.Parameters))
        {
          var collection = StringUtil.GetNameValues(rendering.Parameters, '=', '&');
          foreach (string key in collection)
          {
            urlString[key] = collection[key];
          }
        }

        if (ID.IsID(rendering.Datasource))
        {
          urlString[Constants.PlayerParameters.ItemId] = rendering.Datasource;
        }

        string activePage = args.Parameters[Constants.PlayerParameters.ActivePage];
        if (!string.IsNullOrEmpty(activePage))
        {
          urlString[Constants.PlayerParameters.ActivePage] = activePage;
        }

        Context.ClientPage.ClientResponse.ShowModalDialog(urlString.ToString(), "1100", "600", string.Empty, true);
        args.WaitForPostBack();
      }
      else
      {
        Assert.ArgumentNotNull(args, "args");

        if (args.HasResult)
        {
          var formValue = WebUtil.GetFormValue("scLayout");
          var id = ShortID.Decode(WebUtil.GetFormValue("scDeviceID"));
          var uniqueId = ShortID.Decode(args.Parameters["uniqueid"]);
          var layoutDefinition = WebEditUtil.ConvertJSONLayoutToXML(formValue);
          var parsedLayout = LayoutDefinition.Parse(layoutDefinition);
          var device = parsedLayout.GetDevice(id);
          var deviceIndex = parsedLayout.Devices.IndexOf(device);
          var index = device.GetIndex(uniqueId);
          var rendering = (RenderingDefinition)device.Renderings[index];

          UrlString url = new UrlString(this.GetParameters(args.Result));

          string itemId = url[Constants.PlayerParameters.ItemId];

          url.Remove(Constants.PlayerParameters.ItemId);

          rendering.Datasource = new ID(itemId).ToString();
          rendering.Parameters = url.ToString();


          parsedLayout.Devices[deviceIndex] = device;
          var updatedLayout = parsedLayout.ToXml();
          var layout = GetLayout(updatedLayout);
          SheerResponse.SetAttribute("scLayoutDefinition", "value", layout);
          SheerResponse.Eval("window.parent.Sitecore.PageModes.ChromeManager.handleMessage('chrome:rendering:propertiescompleted');");
        }
      }
    }

    protected virtual string GetParameters(string markup)
    {
      //TODO: Why we need HtmlAgilityPack dependence for simple text parsing?

      var doc = new HtmlAgilityPack.HtmlDocument();
      doc.LoadHtml(markup);
      var element = doc.DocumentNode.ChildNodes["iframe"];

      if (element == null)
      {
        return string.Empty;
      }

      var attr = element.Attributes["src"];
      if (attr != null)
      {
        string[] tmp = attr.Value.Split('?');
        if (tmp.Length > 1)
        {
          return tmp[1];
        }
      }
      return string.Empty;
    }

    /// <summary>
    /// Get Rendering
    /// </summary>
    /// <param name="renderingId">
    /// The rendering Id.
    /// </param>
    /// <returns>
    /// returns Rendering as RenderingDefinition
    /// </returns>
    private static RenderingDefinition GetRendering(string renderingId)
    {
      var formValue = WebUtil.GetFormValue("scLayout");
      var id = ShortID.Decode(WebUtil.GetFormValue("scDeviceID"));
      var layoutDefinition = WebEditUtil.ConvertJSONLayoutToXML(formValue);
      var parsedLayout = LayoutDefinition.Parse(layoutDefinition);
      var device = parsedLayout.GetDevice(id);
      var index = device.GetIndex(renderingId);
      return (RenderingDefinition)device.Renderings[index];
    }

    /// <summary>
    /// Get Layout
    /// </summary>
    /// <param name="layout">
    /// The layout.
    /// </param>
    /// <returns>
    /// returns layout as string
    /// </returns>
    private static string GetLayout(string layout)
    {
      Assert.ArgumentNotNull(layout, "layout");
      return WebEditUtil.ConvertXMLLayoutToJSON(layout);
    }
  }
}