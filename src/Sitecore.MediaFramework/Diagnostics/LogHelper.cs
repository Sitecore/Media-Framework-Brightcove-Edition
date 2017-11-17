
namespace Sitecore.MediaFramework.Diagnostics
{
  using System;

  using Sitecore.Diagnostics;

  public static class LogHelper
  {
    private static readonly string MessagePrefix = "MediaFramework *** ";

    public static void Error(string message, object owner, Exception exception = null)
    {
      Log.Error(GetMessage(message,owner,exception), exception, owner);
    }

    public static void Warn(string message, object owner, Exception exception = null)
    {
      Log.Warn(GetMessage(message, owner, exception), exception, owner);
    }

    public static void Info(string message, object owner)
    {
      Log.Info(GetMessage(message, owner), owner);
    }

    public static void Debug(string message)
    {
      if (MediaFrameworkContext.EnableDebug)
      {
        Log.Debug(GetMessage(message, null));
      }
    }

    public static void Debug(string message, object owner)
    {
      if (MediaFrameworkContext.EnableDebug)
      {
        Log.Debug(GetMessage(message, owner), owner);
      }
    }

    private static string GetMessage(string message, object owner, Exception exception = null)
    {
      string str = MessagePrefix + message;
      
      if (exception == null && owner != null)
      {
        str += "\tCalled by:" + owner.GetType().Name;
      }

      return str;
    }
  }
}