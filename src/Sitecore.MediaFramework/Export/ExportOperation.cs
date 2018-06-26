// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExportOperation.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   The export operation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Export
{
  using System;

  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Account;

  using Sitecore.Data.Items;

  /// <summary>
  /// The export operation.
  /// </summary>
  public class ExportOperation
  {
    public ExportOperation(Item item, ExportOperationType type)
    {
      Assert.ArgumentNotNull(item, "item");

      this.Item = item;
      this.Type = type;
      this.AccountItem = AccountManager.GetAccountItemForDescendant(item);
    }

    /// <summary>
    /// Gets or sets the type of the operation.
    /// </summary>
    public ExportOperationType Type { get; set; }

    /// <summary>
    /// Gets or sets the entity Item.
    /// </summary>
    public Item Item { get; set; }

    public Item AccountItem { get; set; }

    public override string ToString()
    {
      return string.Format("Type:{0};ItemId:{1};AccountItemId:{2}", this.Type, this.Item.ID, this.AccountItem.ID);
    }
  }
}