namespace Sitecore.MediaFramework.Pipelines.RenderLayout
{
  using System;
  using System.Collections.Generic;
  using System.Web.UI;

  using Sitecore.Layouts;
  using Sitecore.Reflection;
  using Sitecore.Web;

  public class TypedRootControlResolver : IRootControlResolver
  {
    protected List<Type> RootTypes { get; private set; }

    public TypedRootControlResolver()
    {
      this.RootTypes = new List<Type>();
    }

    public void AddType(string type)
    {
      Type tmp = ReflectionUtil.GetTypeInfo(type);
      if (tmp != null && !this.RootTypes.Contains(tmp))
      {
        this.RootTypes.Add(tmp);
      }
    }

    public virtual Control GetRootControl()
    {
      PageContext pageContext = Context.Page;
      if (pageContext != null && pageContext.Page != null)
      {
        foreach (Type rootType in this.RootTypes)
        {
          Control root = WebUtil.FindControlOfType(pageContext.Page, rootType);
          if (root != null)
          {
            return root;
          }
        }
      }
      return null;
    }
  }
}