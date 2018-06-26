namespace Sitecore.MediaFramework.Import
{
  using System.Collections.Generic;

  using Sitecore.Data.Items;

  public interface IImportExecuter
  {
    IEnumerable<object> GetData(Item accountItem); 
  }

  //[Obsolete("Use IImportExecuter interface instead of this", false)]
  //public interface IImportData : IImportExecuter
  //{
  //}
}
