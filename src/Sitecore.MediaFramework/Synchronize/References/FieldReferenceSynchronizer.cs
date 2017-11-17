namespace Sitecore.MediaFramework.Synchronize.References
{
  using System;

  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Diagnostics;

  public abstract class FieldReferenceSynchronizer<TEntity, TResult> : IReferenceSynchronizer
    where TResult : class
  {
    public string Field { get; set; }

    public virtual Item SyncReference(object entity, Item accountItem, Item item)
    {
      Field field = this.GetField(item);
      if (field == null)
      {
        return null;
      }

      if (!field.CanWrite)
      {
        LogHelper.Warn(string.Format("Field {0} from item({1}) does not have write permissions", field.Name, field.Item.ID), this);

        return null;
      }

      TResult reference = this.GetReference((TEntity)entity, accountItem);

      if (reference == null)
      {
        return null;
      }

      if (this.NeedUpdate(reference, field.Value))
      {
        LogHelper.Debug(string.Format("Field '{0}' from item({1}) need update", field.Name, field.Item.ID), this);

        string value = this.GetFieldValue(reference);

        return this.UpdateField(field, value);
      }

      LogHelper.Debug(string.Format("Field '{0}' from item({1}) does not need update", field.Name, field.Item.ID), this);

      return null;
    }

    protected abstract TResult GetReference(TEntity entity, Item accountItem);

    protected abstract bool NeedUpdate(TResult reference, string fieldValue);

    protected abstract string GetFieldValue(TResult reference);

    protected virtual Field GetField(Item item)
    {
      Field field = item.Fields[this.Field];
      if (field == null)
      {
        LogHelper.Warn(string.Format("Field '{0}' could not be found", this.Field), this);
      }

      return field;
    }

    protected virtual Item UpdateField(Field field, string value)
    {
      try
      {
        using (new EditContext(field.Item))
        {
          field.Value = value;
        }

        return field.Item;
      }
      catch (Exception ex)
      {
        LogHelper.Error(string.Format("Error while updating field({0}) for item({1})", field.ID, field.Item.ID), this, ex);
        return null;
      }
    }
  }
}