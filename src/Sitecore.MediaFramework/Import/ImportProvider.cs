// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExportProvider.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright> 
// <summary>
//   Help methods for Principal Ingesting
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Import
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Configuration.Provider;
  using System.Linq;

  using Sitecore.Data.Items;

  /// <summary>
  /// Help methods for Principal Ingesting
  /// </summary>
  public class ImportProvider : ProviderBase
  {
    /// <summary>
    /// The import.
    /// </summary>
    /// <param name="importName">
    /// The import Name.
    /// </param>
    /// <param name="accountItem">
    /// The account.
    /// </param>
    /// <returns>
    /// The <see cref="IEnumerable"/>.
    /// </returns>
    [CanBeNull]
    public virtual IEnumerable<object> Import(string importName, Item accountItem)
    {
      IImportExecuter import = MediaFrameworkContext.GetImportExecuter(importName);
      
      return import != null ? import.GetData(accountItem) : null;
    }

    [CanBeNull]
    public virtual List<T> ImportList<T>(string importName, Item accountItem)
    {
      var data = this.Import(importName, accountItem);

      if (data == null)
      {
        return null;
      }

      try
      {
        return data.OfType<T>().ToList();
      }
      catch
      {
        return null;
      }
    }
  }
}