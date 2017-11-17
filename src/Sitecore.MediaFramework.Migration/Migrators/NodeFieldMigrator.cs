namespace Sitecore.MediaFramework.Migration.Migrators
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Sitecore.Data.Fields;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Diagnostics;

  public abstract class NodeFieldMigrator<TDocument, TNode> : FieldMigrator
  {
    public override void MigrateField(Field field)
    {
      Assert.ArgumentNotNull(field, "field");

      var document = this.GetDocument(field);

      bool needUpdate = false;

      var nodes = this.GetNodes(document) ?? Enumerable.Empty<TNode>();

      foreach (TNode node in nodes)
      {
        try
        {
          needUpdate = this.MigrateNode(field, node) || needUpdate;
        }
        catch (Exception ex)
        {
          LogHelper.Error(string.Format("{0}({1}) '{2}' field node migration failed", field.Item.Name, field.Item.ID, field.Name), this, ex);
        }
      }

      if (needUpdate)
      {
        this.UpdateField(field, document);
      }
      else
      {
        LogHelper.Info(string.Format("{0}({1}) '{2}' field does not need migration", field.Item.Name, field.Item.ID, field.Name), this);
      }
    }

    protected abstract IEnumerable<TNode> GetNodes(TDocument document);

    protected abstract bool MigrateNode(Field field, TNode node);

    protected abstract TDocument GetDocument(Field field);

    protected abstract string GetValue(TDocument document);

    protected virtual void UpdateField(Field field, TDocument document)
    {
      Assert.ArgumentNotNull(field,"field");

      LogHelper.Info(string.Format("{0}({1}) '{2}' field needs migration", field.Item.Name, field.Item.ID, field.Name), this);

      if (field.Item.Access.CanWrite())
      {
        field.Item.Editing.BeginEdit();

        field.SetValue(this.GetValue(document), true);

        field.Item.Editing.EndEdit();
      }
      else
      {
        LogHelper.Info(string.Format("{0}({1}) '{2}' field could not be migrated (need write permissions)", field.Item.Name, field.Item.ID, field.Name), this);
      }
    }
  }
}