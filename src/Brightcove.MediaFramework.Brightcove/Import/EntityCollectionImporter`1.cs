
using System;
using System.Collections.Generic;
using System.Web;
using Brightcove.MediaFramework.Brightcove.Configuration;
using Brightcove.MediaFramework.Brightcove.Security;
using RestSharp;
using Sitecore.Data.Items;
using Sitecore.MediaFramework.Diagnostics;
using Sitecore.MediaFramework.Import;

namespace Brightcove.MediaFramework.Brightcove.Import
{
    public abstract class EntityCollectionImporter<TEntity> : IImportExecuter
    {
        public virtual IEnumerable<object> GetData(Item accountItem)
        {
            return this.GetWithPaging((IAuthenticator)new BrightcoveAuthenticator(accountItem));
        }

        protected virtual IEnumerable<object> GetWithPaging(IAuthenticator authenticator)
        {
            int limit = Settings.ImportLimit;
            int offset = -limit;
        label_1:
            offset = offset + limit;

            Entities.Collections.PagedCollection<TEntity> temp = this.RetrieveList(limit, offset, authenticator);
            if (temp == null)
            {
                LogHelper.Warn("Null Result during importing", (object)this, (Exception)null);
                throw new HttpException("Http null result");
            }

            if (temp.Count != 0)
            {
                foreach (TEntity entity in temp)
                    yield return (object)entity;

                if (temp.Count>=limit)
                    goto label_1;
            }
        }

        public abstract Entities.Collections.PagedCollection<TEntity> RetrieveList(int limit, int offset, IAuthenticator authenticator);
    }
}
