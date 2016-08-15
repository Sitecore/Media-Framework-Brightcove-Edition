using System.Collections.Generic;
using System.Web;

using Sitecore.Data.Items;
using Sitecore.MediaFramework.Brightcove.Entities.Collections;
using Sitecore.MediaFramework.Brightcove.Security;
using Sitecore.MediaFramework.Diagnostics;
using Sitecore.MediaFramework.Import;
using Sitecore.RestSharp;
using RestSharp;
using RestSharp.Authenticators;

namespace Sitecore.MediaFramework.Brightcove.Import
{
  public abstract class EntityCollectionImporter<TEntity> : IImportExecuter
  {
    protected abstract string RequestName { get; }

    public virtual IEnumerable<object> GetData(Item accountItem)
    {
      var authenticator = new BrightcoveAthenticator(accountItem);

      return this.GetWithPaging(authenticator);
    }

    protected virtual IEnumerable<object> GetWithPaging(IAuthenticator authenticator)
    {
      var context = new RestContext(Constants.SitecoreRestSharpService, authenticator);

      var parameters = new List<Parameter>();

      var pageNumber = new Parameter { Type = ParameterType.UrlSegment, Name = "page_number", Value = -1 };
      
      parameters.Add(pageNumber);

      int totalCount;
      int count = 0;

      do
      {
        pageNumber.Value = (int)pageNumber.Value + 1;

        var temp = context.Read<PagedCollection<TEntity>>(this.RequestName, parameters);

        if (temp == null || temp.Data == null || temp.Data.Items == null)
        {
          LogHelper.Warn("Null Result during importing", this);
          
          throw new HttpException("Http null result");
        }

        totalCount = temp.Data.TotalCount;

        if (temp.Data.Items.Count == 0)
        {
          yield break;
        }

        foreach (TEntity entity in temp.Data.Items)
        {
          yield return entity;
        }

        count += temp.Data.Items.Count;
      }
      while (count < totalCount);
    }
  }
}