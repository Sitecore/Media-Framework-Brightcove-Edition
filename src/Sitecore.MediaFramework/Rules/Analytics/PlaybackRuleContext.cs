
namespace Sitecore.MediaFramework.Rules.Analytics
{
  using System.Collections.Generic;

  using Sitecore.Rules;

  public class PlaybackRuleContext : RuleContext
  {
    public PlaybackRuleContext()
    {
      this.PlaybackEvents = new Dictionary<string, List<string>>();
    }

    public Dictionary<string, List<string>> PlaybackEvents { get; protected set; }

    public void AddPlaybackEvent(string name, string parameter = "", bool replace = false)
    {
      if (replace || !this.PlaybackEvents.ContainsKey(name))
      {
        this.PlaybackEvents[name] =  new List<string> { parameter };
      }
      else
      {
        List<string> parameters = this.PlaybackEvents[name];
        if (!parameters.Contains(parameter))
        {
          parameters.Add(parameter);
        }
      }
    }
  }
}