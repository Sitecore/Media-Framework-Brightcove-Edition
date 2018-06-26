namespace Sitecore.MediaFramework.Commands
{
  using System;
  using System.Collections.Specialized;

  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Sitecore.Diagnostics;
  using Sitecore.Globalization;
  using Sitecore.MediaFramework.Account;
  using Sitecore.MediaFramework.Entities;
  using Sitecore.MediaFramework.Utils;
  using Sitecore.Shell.Framework.Commands;
  using Sitecore.Web.UI.Sheer;

  [Serializable]
  public class CreateEntityItem : Command
  {
    protected virtual string EntityIdValidation
    {
      get
      {
        return "^.{1,200}$";
      }
    }

    public override void Execute(CommandContext context)
    {
      if (context.Items.Length == 1)
      {
        Item item = context.Items[0];

        NameValueCollection parameters = new NameValueCollection();
        parameters["parentId"] = item.ID.ToString();
        parameters["language"] = item.Language.ToString();
        parameters["database"] = item.Database.Name;
        parameters["templateId"] = context.Parameters["templateId"];
        parameters["entityIdField"] = context.Parameters["entityIdField"];
        parameters["entityNameField"] = context.Parameters["entityNameField"];

        Context.ClientPage.Start(this, "Run", parameters);
      }
    }

    protected virtual void Run(ClientPipelineArgs args)
    {
      //Cancel button
      if (args.IsPostBack && !args.HasResult)
      {
        return;
      }

      Database db = Factory.GetDatabase(args.Parameters["database"]);

      TemplateItem templateItem = db.Templates[this.GetTemplateId(args)];
      Item parent = db.GetItem(this.GetParentId(args));
      
      if (templateItem == null || parent == null)
      {
        return;
      }

      if (string.IsNullOrEmpty(this.GetEntityName(args)))
      {
        if (args.HasResult)
        {
          this.SetEntityName(args, args.Result);

          args.Result = "undefined";
        }
        else
        {
          Context.ClientPage.ClientResponse.Input(
            Translate.Text(Texts.ENTER_THE_NAME_OF_THE_NEW_ITEM),
            templateItem.DisplayName,
            Settings.ItemNameValidation,
            Translate.Text(Texts.INPUT_IS_NOT_A_VALID_NAME),
            100);
          args.WaitForPostBack();
        }
      }

      if (!string.IsNullOrEmpty(this.GetEntityName(args)) && string.IsNullOrEmpty(this.GetEntityId(args)))
      {
        if (args.HasResult)
        {
          this.SetEntityId(args, args.Result);

          args.Result = "undefined";
        }
        else
        {
          Context.ClientPage.ClientResponse.Input(
            Translate.Text(Translations.EnterTheEntityId),
            string.Empty,
            this.EntityIdValidation,
            Translate.Text(Texts.INPUT_IS_NOT_A_VALID_NAME),
            100);
          args.WaitForPostBack();
        }
      }

      string entityName = this.GetEntityName(args);
      string entityId = this.GetEntityId(args);

      if (string.IsNullOrEmpty(entityName) || string.IsNullOrEmpty(entityId))
      {
        return;
      }

      Item accountItem = AccountManager.GetAccountItemForDescendant(parent);
      
      if (accountItem == null)
      {
        return;
      }

      ID itemId = this.GenerateItemId(args, accountItem);

      Item item = ItemManager.AddFromTemplate(entityName, templateItem.ID, parent, itemId);

      if (item == null)
      {
        return;
      }

      using (new EditContext(item))
      {
        string entityIdField = this.GetEntityIdField(args);

        Assert.IsNotNullOrEmpty(entityIdField, "entityIdField is null or empty");

        item[entityIdField] = entityId;

        string entityNameField = this.GetEntityNameField(args);

        if (!string.IsNullOrEmpty(entityNameField))
        {
          item[entityNameField] = entityName;
        }
      }
    }

    protected virtual MediaServiceEntityData GetMediaData(ClientPipelineArgs args)
    {
      return new MediaServiceEntityData
        {
          EntityId = this.GetEntityId(args),
          EntityName = this.GetEntityName(args),
          TemplateId = this.GetTemplateId(args)
        };
    }

    protected virtual ID GenerateItemId(ClientPipelineArgs args, Item accountItem)
    {
      var mediaData = this.GetMediaData(args);

      return IdUtil.GenerateItemId(accountItem, mediaData);
    }

    protected virtual ID GetParentId(ClientPipelineArgs args)
    {
      string str = args.Parameters["parentId"];
      return ID.IsID(str) ? new ID(str) : ID.Null;
    }

    protected virtual ID GetTemplateId(ClientPipelineArgs args)
    {
      string str = args.Parameters["templateId"];
      return ID.IsID(str) ? new ID(str) : ID.Null;
    }

    protected virtual string GetEntityId(ClientPipelineArgs args)
    {
      return args.Parameters["entityId"];
    }

    protected virtual void SetEntityId(ClientPipelineArgs args, string value)
    {
      args.Parameters["entityId"] = value;
    }

    protected virtual string GetEntityName(ClientPipelineArgs args)
    {
      return args.Parameters["entityName"];
    }

    protected virtual void SetEntityName(ClientPipelineArgs args, string value)
    {
      args.Parameters["entityName"] = value;
    }

    protected virtual string GetEntityIdField(ClientPipelineArgs args)
    {
      return args.Parameters["entityIdField"];
    }
    protected virtual string GetEntityNameField(ClientPipelineArgs args)
    {
      return args.Parameters["entityNameField"];
    }
  }
}