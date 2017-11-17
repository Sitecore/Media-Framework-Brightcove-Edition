namespace Sitecore.MediaFramework.UI.Sublayouts
{
  using System;
  using System.Collections.Generic;
  using System.Linq;                       
  using System.Web.UI;

  using Newtonsoft.Json;

  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Globalization;
  using Sitecore.MediaFramework.Account;
  using Sitecore.MediaFramework.Common;
  using Sitecore.MediaFramework.Diagnostics;

  public partial class Upload : Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        wrongExtension.Value = Translate.Text(Translations.FileUploadingWrongExtension);
        TextGoTo.Value = Translate.Text(Translations.GoToItem);
        TextSelectItem.Value = Translate.Text(Translations.SelectItem);
        AccountMenuWarn.Value = Translate.Text(Translations.AccountMenuWarn);
        EmptyResult.Value = Translate.Text(Translations.EmptyFileUploadResult);
        Buffered.Value = Translate.Text(Translations.Buffered);

        ltrAccounts.Text = Translate.Text(Translations.Accounts);
        ltrAddFiles.Text = Translate.Text(Translations.AddFiles);
        ltrStartUpload.Text = Translate.Text(Translations.StartUpload);      
        ltrStart.Text = Translate.Text(Translations.Start);
        ltrError.Text = Translate.Text(Translations.Error);
        ltrBuffering.Text = Translate.Text(Translations.Buffering);
        ltrCancel.Text = Translate.Text(Translations.Cancel);
        this.PageData.Value = this.GetAccountData();    
      }            
    }

    protected virtual Item GetAccountItem()
    {                                                                            
      var itemId = Page.Request.QueryString.Get("itemId");
      var db = Page.Request.QueryString.Get("database");

      if (!Sitecore.Data.ID.IsID(itemId) || string.IsNullOrEmpty(db))
      {
        return null;
      }

      var item = Database.GetDatabase(db).GetItem(itemId);
      return item != null ? AccountManager.GetAccountItemForDescendant(item) : null;
    }
         
    /// <summary>
    /// The get account data in json format.
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    protected virtual string GetAccountData()
    {
      var type = Request.QueryString.Get("type");
     
      var dbName = Page.Request.QueryString.Get("database");

      Database db = string.IsNullOrEmpty(dbName)
                      ? Sitecore.Context.ContentDatabase ?? Sitecore.Context.Database
                      : Database.GetDatabase(dbName);
   
      Item accountItem = this.GetAccountItem();

      var accs = AccountManager.GetAllAccounts(db).Where(AccountManager.IsValidAccount).ToList();
      if (accs.Count == 0)
      {
        LogHelper.Warn("Media Framework has no Accounts!", this);
      }

      var accountsList = accs.Select(acc => this.SetAccountProperties(acc, accountItem != null && acc.ID == accountItem.ID)).GroupBy(it => it.AccountTemplateId).Where(t => this.CheckAccount(t.Key)).ToList();
      
      var data = new PageProperties
       {
         NoAcc = accountItem == null,
         AllAccounts = accountsList,
         Database = db.Name,
         Mode = string.IsNullOrEmpty(type) ? "embed" : type
       };

       return JsonConvert.SerializeObject(data);
    }

    protected virtual Account SetAccountProperties(Item account, bool selected)
    {
      return new Account(account, selected);
    }

    protected virtual bool CheckAccount(Guid accountTemplateId)
    {
      return MediaFrameworkContext.GetUploadExecuter(new ID(accountTemplateId)) != null;
    }

    public class PageProperties
    {
      [JsonProperty("noAcc")]
      public bool NoAcc { get; set; }
      [JsonProperty("database")]
      public string Database { get; set; }
      [JsonProperty("allAccounts")]
      public List<IGrouping<Guid, Account>> AllAccounts { get; set; }
      [JsonProperty("mode")]
      public string Mode { get; set; } 
    } 
  }
}