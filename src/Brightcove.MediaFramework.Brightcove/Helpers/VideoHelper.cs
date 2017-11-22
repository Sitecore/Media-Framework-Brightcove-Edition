using Brightcove.MediaFramework.Brightcove.Extensions;
using Sitecore.MediaFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Brightcove.MediaFramework.Brightcove.Indexing.Entities;
using Brightcove.MediaFramework.Brightcove.Proxy.CMS;
using Brightcove.MediaFramework.Brightcove.Security;
using Sitecore.Configuration;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Integration.Common.Utils;
using Sitecore.MediaFramework.Account;

using Sitecore.SecurityModel;
using Brightcove.MediaFramework.Brightcove.Entities;

namespace Brightcove.MediaFramework.Brightcove.Helpers
{
    public static class VideoHelper
    {
        public static MediaServiceSearchResult GetSearchResult(string entity, Item ancesterItem = null)
        {
            if (ancesterItem == null) ancesterItem = Factory.GetDatabase("master").GetItem(Sitecore.MediaFramework.ItemIDs.AccountsRoot);
            return (MediaServiceSearchResult)GetSearchResultForAncesterFilter<VideoSearchResult>(Configuration.Settings.IndexName, ancesterItem, (Expression<Func<VideoSearchResult, bool>>)(i => i.TemplateId == TemplateIDs.Video && i.Id == entity));
        }

        public static TSearchResult GetSearchResultForAncesterFilter<TSearchResult>(string indexName, Item ancesterItem, Expression<Func<TSearchResult, bool>> selector) where TSearchResult : MediaServiceSearchResult, new()
        {
            Expression<Func<TSearchResult, bool>> ancestorFilter = ContentSearchUtil.GetAncestorFilter<TSearchResult>(ancesterItem, (List<ID>)null);
            return ContentSearchUtil.FindOne<TSearchResult>(indexName, PredicateBuilder.And<TSearchResult>(ancestorFilter, selector));
        }

        public static void SetIngestSuccessfulField(this MediaServiceSearchResult item, string value)
        {
            if (item == null) return;
            var sitecoreItem = item.GetItem();
            using (new SecurityDisabler())
            {
                try
                {
                    sitecoreItem.Editing.BeginEdit();
                    sitecoreItem[FieldIDs.Video.IngestStatus] = value;
                    sitecoreItem.Editing.EndEdit();
                }
                catch (Exception ex)
                {
                    //Revert the Changes
                    sitecoreItem.Editing.CancelEdit();
                }
            }
        }

        public static Item GetAccountItem(Item item)
        {
            return AccountManager.GetAccountItemForDescendant(item);
        }

        public static void SetIngestStatus(Notification notification)
        {
            if (notification == null) return;
            if (!notification.IsVideoUpload()) return;
            var videoItem = GetSearchResult(notification.Entity);
            if (videoItem == null) return;
            var accountItem = GetAccountItem(videoItem.GetItem());
            if (accountItem == null) return;

            string fieldValue;
            if (notification.IsSuccess())
            {
                var authenticator = new BrightcoveAuthenticator(accountItem);
                var proxy = new VideoProxy(authenticator);
                var response = proxy.RetrieveSourcesById(notification.Entity);

                if (response != null && response.Count > 0)
                {
                    fieldValue = response.Count + " sources found.";
                }
                else
                {
                    fieldValue = "No sources found.";
                }
            }
            else
            {
                fieldValue = "Ingest Failed";
            }

            // Set the ingest successful field value
            videoItem.SetIngestSuccessfulField(fieldValue);
        }
    }
}
