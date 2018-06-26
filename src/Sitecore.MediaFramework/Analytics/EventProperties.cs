namespace Sitecore.MediaFramework.Analytics
{
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Globalization;

  using Sitecore.Data;
  using Sitecore.MediaFramework.Players;

  public class EventProperties : PlayerProperties
  {
    public EventProperties(Dictionary<string, string> dictionary) : base(dictionary)
    {
    }

    public EventProperties(NameValueCollection collection) : base(collection)
    {
    }

    public ID ContextItemId
    {
      get
      {
        return GetId(this.Collection[Constants.PlayerEventParameters.ContextItemId]);
      }
      set
      {
        this.Collection[Constants.PlayerEventParameters.ContextItemId] = value.ToShortID().ToString();
      }
    }

    public string MediaName
    {
      get
      {
        return this.Collection[Constants.PlayerEventParameters.MediaName];
      }
      set
      {
        this.Collection[Constants.PlayerEventParameters.MediaName] = value;
      }
    }

    public int MediaLength
    {
      get
      {
        return MainUtil.GetInt(this.Collection[Constants.PlayerEventParameters.MediaLength], 0);
      }
      set
      {
        this.Collection[Constants.PlayerEventParameters.MediaLength] = value.ToString(CultureInfo.InvariantCulture);
      }
    }

    public string EventParameter
    {
      get
      {
        return this.Collection[Constants.PlayerEventParameters.EventParameter];
      }
      set
      {
        this.Collection[Constants.PlayerEventParameters.EventParameter] = value;
      }
    }
  }
}