using Sitecore;
using Sitecore.MediaFramework;

namespace Brightcove.MediaFramework.Brightcove.Players
{
  using System;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Globalization;
  using System.Linq;

  using Sitecore.Data;
  using Sitecore.Diagnostics;

  [Serializable]
  public class BrightcovePlayerProperties
  {
    public BrightcovePlayerProperties()
    {
      this.Collection = new NameValueCollection();
    }

    public BrightcovePlayerProperties(Dictionary<string, string> dictionary) :this()
    {
      foreach (var pair in dictionary)
      {
        this.Collection.Add(pair.Key, pair.Value);
      }
    }

    public BrightcovePlayerProperties(NameValueCollection collection)
    {
      Assert.ArgumentNotNull(collection, "collection");

      this.Collection = new NameValueCollection(collection);
    }

    public NameValueCollection Collection { get; protected set; }

    public ID Template
    {
      get
      {
        return GetId(this.Collection[BrightcovePlayerParameters.Template]);
      }
      set
      {
        this.Collection[BrightcovePlayerParameters.Template] = !ReferenceEquals(value, null) ? value.ToShortID().ToString() : null;
      }
    }

    public ID ItemId
    {
      get
      {
        return GetId(this.Collection[BrightcovePlayerParameters.ItemId]);
      }
      set
      {
        this.Collection[BrightcovePlayerParameters.ItemId] = !ReferenceEquals(value, null) ? value.ToShortID().ToString() : null;
      }
    }

    public ID PlayerId
    {
      get
      {
        return GetId(this.Collection[BrightcovePlayerParameters.PlayerId]);
      }
      set
      {
        this.Collection[BrightcovePlayerParameters.PlayerId] = !ReferenceEquals(value, null) ? value.ToShortID().ToString() : null;
      }
    }

    public string MediaId
    {
      get
      {
        return this.Collection[BrightcovePlayerParameters.MediaId];
      }
      set
      {
        this.Collection[BrightcovePlayerParameters.MediaId] = value;
      }
    }
      
    public int Width
    {
      get
      {
        return MainUtil.GetInt(this.Collection[BrightcovePlayerParameters.Width], MediaFrameworkContext.DefaultPlayerSize.Width);
      }
      set
      {
        this.Collection[BrightcovePlayerParameters.Width] = value.ToString(CultureInfo.InvariantCulture);
      }
    }

    public int Height                                             
    {
      get
      {
        return MainUtil.GetInt(this.Collection[BrightcovePlayerParameters.Height], MediaFrameworkContext.DefaultPlayerSize.Height);
      }
      set
      {
        this.Collection[BrightcovePlayerParameters.Height] = value.ToString(CultureInfo.InvariantCulture);
      }
    }

    public string Source
    {
      get
      {
        return this.Collection[BrightcovePlayerParameters.Source];
      }
      set
      {
        this.Collection[BrightcovePlayerParameters.Source] = value.ToString(CultureInfo.InvariantCulture);
      }
    }

    public bool Autoplay
    {
      get
      {
        return this.Collection[BrightcovePlayerParameters.Autoplay] == "1";
      }
      set
      {
        this.Collection[BrightcovePlayerParameters.Autoplay] = value ? "1" : "0";
      }
    }

    public bool Muted
    {
      get
      {
        return this.Collection[BrightcovePlayerParameters.Muted] == "1";
      }
      set
      {
        this.Collection[BrightcovePlayerParameters.Muted] = value ? "1" : "0";
      }
    }

    public string EmbedStyle
    {
      get
      {
        return this.Collection[BrightcovePlayerParameters.EmbedStyle];
      }
      set
      {
        this.Collection[BrightcovePlayerParameters.EmbedStyle] = value;
      }
    }

    public string Sizing
    {
      get
      {
        return this.Collection[BrightcovePlayerParameters.Sizing];
      }
      set
      {
        this.Collection[BrightcovePlayerParameters.Sizing] = value;
      }
    }

    public string Shortcode
    {
      get
      {
        return this.Collection[BrightcovePlayerParameters.Shortcode];
      }
      set
      {
        this.Collection[BrightcovePlayerParameters.Shortcode] = value;
      }
    }

    public bool ForceRender
    {
      get
      {
        return this.Collection[BrightcovePlayerParameters.ForceRender] == "1";
      }
      set
      {
        this.Collection[BrightcovePlayerParameters.ForceRender] = value ? "1" : "0";
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