namespace Sitecore.MediaFramework.Synchronize.References
{
  using Sitecore.MediaFramework.Entities;

  public abstract class StringReferenceSynchronizer<TEntity> : FieldReferenceSynchronizer<TEntity,string>
  {
    protected override bool NeedUpdate(string reference, string fieldValue)
    {
      return reference != fieldValue;
    }

    protected override string GetFieldValue(string reference)
    {
      return reference;
    }
  }
}