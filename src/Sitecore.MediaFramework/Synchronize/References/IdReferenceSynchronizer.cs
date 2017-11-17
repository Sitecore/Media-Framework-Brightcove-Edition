namespace Sitecore.MediaFramework.Synchronize.References
{
  using System.Collections.Generic;
  using System.Linq;

  using Sitecore.Data;
  using Sitecore.Diagnostics;

  public abstract class IdReferenceSynchronizer<TEntity> : FieldReferenceSynchronizer<TEntity, List<ID>>
  {
    protected override bool NeedUpdate(List<ID> reference, string fieldValue)
    {
      Assert.ArgumentNotNull(reference, "reference");

      ISet<ID> referenceSet = new HashSet<ID>(reference.Where(id => !ReferenceEquals(id, null)));

      return !referenceSet.SetEquals(ID.ParseArray(fieldValue));
    }

    protected override string GetFieldValue(List<ID> reference)
    {
      Assert.ArgumentNotNull(reference, "reference");

      return ID.ArrayToString(reference.Where(id => !ReferenceEquals(id,null)).ToArray());
    }
  }
}