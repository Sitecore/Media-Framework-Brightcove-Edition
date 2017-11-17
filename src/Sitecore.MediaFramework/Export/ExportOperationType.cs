namespace Sitecore.MediaFramework.Export
{
  using System;

  [Flags]
  public enum ExportOperationType
  {
    Create = 1,
    Update = 2,
    Delete = 4,
    Move = 8
  }
}