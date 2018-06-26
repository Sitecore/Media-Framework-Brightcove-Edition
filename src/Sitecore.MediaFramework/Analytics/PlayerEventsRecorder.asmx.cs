// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerEventsRecorder.asmx.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   Player events recorder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Analytics
{
  using System.Collections.Generic;
  using System.Web.Script.Services;
  using System.Web.Services;
  using System.Web.Services.Protocols;

  using Newtonsoft.Json;

  using Sitecore.MediaFramework.Pipelines.Analytics;

  /// <summary>
  /// Player events recorder.
  /// </summary>
  [WebService]
  [ScriptService]
  public class PlayerEventsRecorder : WebService
  {
    /// <summary>
    /// Records an event.
    /// </summary>
    /// <param name="eventName">
    /// The event name.
    /// </param>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public void RecordEvent(string eventName, string parameters)
    {
      var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(parameters);

      if (dict != null)
      {
        var args = new PlayerEventArgs { EventName = eventName, Properties = new EventProperties(dict) };
        PlayerEventPipeline.Run(args);

        if (args.Aborted)
        {
          throw new SoapException("Pipeline has been aborted",SoapException.ServerFaultCode);
        }
      }
      else
      {
        throw new SoapException("Parameters could not be parsed", SoapException.ServerFaultCode);
      }
    }
  }
}