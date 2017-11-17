// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemExtensions.cs" company="Sitecore A/S">
//   Copyright (C) 2013 by Sitecore A/S
// </copyright>
// <summary>
//   The item extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.MediaFramework.Common
{
  using System.Linq;

  using Sitecore.Data.Items;
  using Sitecore.Data.Validators;
  using Sitecore.MediaFramework.Export;
  using Sitecore.Pipelines.Save;

  /// <summary>
  /// The item extensions.
  /// </summary>
  public static class ItemExtensions
  {
    public static bool IsValidItem(this Item item, ValidatorsMode validationMode = ValidatorsMode.ValidatorBar)
    {
      if (item == null)
      {
        return false;
      }

      var validators = item.GetValidationResult(validationMode);

      return !validators.Any(v => !v.IsEvaluating && v.Result != ValidatorResult.Valid && v.Result != ValidatorResult.Unknown);
    }

    public static ValidatorCollection GetValidationResult(this Item item, ValidatorsMode validationMode = ValidatorsMode.ValidatorBar)
    {
      var validators = ValidatorManager.BuildValidators(validationMode, item);
      if (validators.Count == 0)
      {
        return new ValidatorCollection();
      }

      ValidatorOptions options = new ValidatorOptions(true);
      ValidatorManager.Validate(validators, options);

      return validators;
    }

    public static bool ExportCreate(this Item item)
    {
      if (item == null)
      {
        return false;
      }

      IExportExecuter executer = MediaFrameworkContext.GetExportExecuter(item);
      if (executer != null && executer.IsNew(item))
      {
        executer.Export(new ExportOperation(item, ExportOperationType.Create));
        return true;
      }

      return false;
    }

    public static bool ExportUpdate(this Item item, SaveArgs.SaveItem saveItem = null)
    {
      if (item == null)
      {
        return false;
      }

      IExportExecuter executer = MediaFrameworkContext.GetExportExecuter(item);
      if (executer != null && !item.ExportCreate() && (saveItem == null || executer.NeedToUpdate(saveItem)))
      {
        ExportQueueManager.Add(new ExportOperation(item, ExportOperationType.Update));
        return true;
      }
      return false;
    }

    public static bool ExportMove(this Item item)
    {
      if (item == null)
      {
        return false;
      }

      IExportExecuter executer = MediaFrameworkContext.GetExportExecuter(item);
      if (executer != null && !item.ExportCreate())
      {
        ExportQueueManager.Add(new ExportOperation(item, ExportOperationType.Move));
        return true;
      }

      return false;
    }

    public static bool ExportDelete(this Item item)
    {
      if (item == null)
      {
        return false;
      }

      IExportExecuter executer = MediaFrameworkContext.GetExportExecuter(item);
      if (executer != null && !executer.IsNew(item))
      {
        ExportQueueManager.Add(new ExportOperation(item, ExportOperationType.Delete));
        return true;
      }
      return false;
    }
  }
}