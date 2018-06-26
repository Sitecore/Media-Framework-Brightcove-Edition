// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteMediaServiceEntity.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   Provide triggering and sending the delete export operations for services.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Pipelines.DeleteUI
{
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Common;
  using Sitecore.Web.UI.Sheer;

  /// <summary>
  /// Provide triggering and sending the delete export operations for services.
  /// </summary>
  public class ExportItems
  {
    /// <summary>
    /// Processes export operation preparing and sending.
    /// </summary>
    /// <param name="args">The args.</param>
    public virtual void Process(ClientPipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.Parameters, "args.Parameters");

      string databaseName = args.Parameters["database"];

      if (!MediaFrameworkContext.IsExportAllowed(databaseName))
      {
        return;
      }

      Database database = Factory.GetDatabase(databaseName);

      ID[] ids = ID.ParseArray(args.Parameters["items"]);

      foreach (ID itemId in ids)
      {
        Item item = database.GetItem(itemId);

        item.ExportDelete();
      }
    }
  }
}