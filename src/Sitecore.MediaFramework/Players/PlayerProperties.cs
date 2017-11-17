namespace Sitecore.MediaFramework.Players
{
  using System;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Globalization;
  using System.Linq;

  using Sitecore.Data;
  using Sitecore.Diagnostics;

  [Serializable]
  public class PlayerProperties
  {
    public PlayerProperties()
    {
      this.Collection = new NameValueCollection();
    }

    public PlayerProperties(Dictionary<string, string> dictionary) :this()
    {
      foreach (var pair in dictionary)
      {
        this.Collection.Add(pair.Key, pair.Value);
      }
    }

    public PlayerProperties(NameValueCollection collection)
    {
      Assert.ArgumentNotNull(collection, "collection");

      this.Collection = new NameValueCollection(collection);
    }

    public NameValueCollection Collection { get; protected set; }

    public ID Template
    {
      get
      {
        return GetId(this.Collection[Constants.PlayerParameters.Template]);
      }
      set
      {
        this.Collection[Constants.PlayerParameters.Template] = !ReferenceEquals(value, null) ? value.ToShortID().ToString() : null;
      }
    }

    public ID ItemId
    {
      get
      {
        return GetId(this.Collection[Constants.PlayerParameters.ItemId]);
      }
      set
      {
        this.Collection[Constants.PlayerParameters.ItemId] = !ReferenceEquals(value, null) ? value.ToShortID().ToString() : null;
      }
    }

    public ID PlayerId
    {
      get
      {
        return GetId(this.Collection[Constants.PlayerParameters.PlayerId]);
      }
      set
      {
        this.Collection[Constants.PlayerParameters.PlayerId] = !ReferenceEquals(value, null) ? value.ToShortID().ToString() : null;
      }
    }

    public string MediaId
    {
      get
      {
        return this.Collection[Constants.PlayerParameters.MediaId];
      }
      set
      {
        this.Collection[Constants.PlayerParameters.MediaId] = value;
      }
    }
      
    public int Width
    {
      get
      {
        return MainUtil.GetInt(this.Collection[Constants.PlayerParameters.Width], MediaFrameworkContext.DefaultPlayerSize.Width);
      }
      set
      {
        this.Collection[Constants.PlayerParameters.Width] = value.ToString(CultureInfo.InvariantCulture);
      }
    }

    public int Height                                             
    {
      get
      {
        return MainUtil.GetInt(this.Collection[Constants.PlayerParameters.Height], MediaFrameworkContext.DefaultPlayerSize.Height);
      }
      set
      {
        this.Collection[Constants.PlayerParameters.Height] = value.ToString(CultureInfo.InvariantCulture);
      }
    }

    public bool ForceRender
    {
      get
      {
        return this.Collection[Constants.PlayerParameters.ForceRender] == "1";
      }
      set
      {
        this.Collection[Constants.PlayerParameters.ForceRender] = value ? "1" : "0";
      }
    }

    public override string ToString()
    {
      return string.Join("&", this.Collection.AllKeys.Select(i => i + "=" + this.Collection[i]));
    }

    protected static ID GetId(string value)
    {
      return !string.IsNullOrEmpty(value) && ShortID.IsShortID(value) ? new ID(value) : ID.Null;
    }
  }
}