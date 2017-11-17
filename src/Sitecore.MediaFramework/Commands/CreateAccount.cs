// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateAccount.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   Defines the CreateAccount type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Commands
{
  using System;
  using System.Collections.Specialized;

  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Globalization;
  using Sitecore.MediaFramework.Account;
  using Sitecore.Shell.Framework.Commands;
  using Sitecore.Web.UI.Sheer;

  /// <summary>
  /// The create a service account.
  /// </summary>
  [Serializable]
  public class CreateAccount : Command
  {
    /// <summary>
    /// The execute.
    /// </summary>
    /// <param name="context">
    /// The context.
    /// </param>
    public override void Execute(CommandContext context)
    {
      if (context.Items.Length == 1)
      {
        Item item = context.Items[0];

        NameValueCollection parameters = new NameValueCollection();
        parameters["language"] = item.Language.ToString();
        parameters["database"] = item.Database.Name;
        parameters["branch"] = context.Parameters["branch"];
        parameters["settings"] = context.Parameters["settings"];

        Context.ClientPage.Start(this, "Run", parameters);
      }
    }

    /// <summary>
    /// The run.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    protected virtual void Run(ClientPipelineArgs args)
    {
      Database db = Factory.GetDatabase(args.Parameters["database"]);

      BranchItem branchItem = db.Branches[new ID(args.Parameters["branch"])];

      if (!args.IsPostBack)
      {
        // Means we are in the initial step, we want to ask for the name of the news
        Context.ClientPage.ClientResponse.Input(
          Translate.Text(Texts.ENTER_THE_NAME_OF_THE_NEW_ITEM),
          branchItem.Name,
          Settings.ItemNameValidation,
          Translate.Text(Texts.INPUT_IS_NOT_A_VALID_NAME),
          100);

        args.WaitForPostBack();
      }
      else if (args.HasResult)
      {
        Item accountsRoot = AccountManager.GetAccountsRoot(db);
        Item settings = db.GetItem(args.Parameters["settings"]);

        if (accountsRoot != null && settings != null)
        {
          Item account = accountsRoot.Add(args.Result, branchItem);

          Item accountSettings = settings.CloneTo(account, "Settings", true);
          using (new EditContext(accountSettings))
          {
            accountSettings[Sitecore.FieldIDs.DisplayName] = Translate.Text(Translations.Settings);
          }


          Context.ClientPage.ClientResponse.Timer(string.Format("item:load(id={0})", account.ID), 100);
        }
      }
    }
  }
}