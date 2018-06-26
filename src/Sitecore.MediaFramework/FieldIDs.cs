namespace Sitecore.MediaFramework
{
  using Sitecore.Data;

  public static class FieldIDs
  {
    public static class AccountSettings
    {
      public static readonly ID PlaybackEventsRules = new ID("{12112D15-A3DE-4B42-A700-DE1B9666391B}");
    }

    public static class MediaElement
    {
      public static readonly ID Events = new ID("{C923961C-5664-43D2-85D2-D194143197DF}");
    }

    public static class PlaybackEvent
    {
      public static readonly ID PageEvent = new ID("{7CCB928C-7EB0-465A-A755-D4BD7256BD58}");
      public static readonly ID Parameter = new ID("{E1E68B6B-5C30-41D3-B6C9-7089B5CE1B34}");
    }
  }
}