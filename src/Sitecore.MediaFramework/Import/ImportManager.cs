// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImportManager.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright> 
// <summary>
// Ingest provides feeding sitecore content by Ingesting on AIE
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Import
{
  using System.Collections.Generic;

  using Sitecore.Configuration;                                                           
  using Sitecore.Data.Items;
  using Sitecore.Integration.Common.Providers;

  /// <summary>
  /// Ingest provides feeding sitecore content by Ingesting on AIE
  /// </summary>
  public static class ImportManager
  { 
    #region Initialization 
    
    /// <summary>
    /// Initializes static members of the <see cref="ImportManager"/> class.
    /// </summary>
    static ImportManager()
    {
      var helper = new ProviderHelper<ImportProvider, ProviderCollection<ImportProvider>>("mediaFramework/importManager");
      Providers = helper.Providers;
      Provider = helper.Provider;
    }

    /// <summary>
    /// Gets or sets the provider.
    /// </summary>
    public static ImportProvider Provider { get; set; }

    /// <summary>
    /// Gets the providers.
    /// </summary>
    public static ProviderCollection<ImportProvider> Providers { get; private set; }

    #endregion

    [CanBeNull]
    public static IEnumerable<object> Import(string importName, Item accountItem)
    {
      return Provider.Import(importName, accountItem);
    }

    [CanBeNull]
    public static List<T> ImportList<T>(string importName, Item accountItem)
    {
      return Provider.ImportList<T>(importName, accountItem);
    }
  }
}