namespace Sitecore.MediaFramework.Commands
{
  using System;

  [Flags]
  public enum ActionType
  {
    Import = 1,
    CleanUp = 2
  }
}