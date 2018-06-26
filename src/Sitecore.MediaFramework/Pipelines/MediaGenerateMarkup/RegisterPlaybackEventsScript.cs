namespace Sitecore.MediaFramework.Pipelines.MediaGenerateMarkup
{
  using Newtonsoft.Json;

  using Sitecore.Diagnostics;

  public class RegisterPlaybackEventsScript : MediaGenerateMarkupProcessorBase
  {
    public override void Process(MediaGenerateMarkupArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.MediaItem, "args.PlayerProperties");
      Assert.ArgumentNotNull(args.PlaybackEvents, "args.PlaybackEvents");

      if (args.PlaybackEvents.Count > 0)
      {
        string mediaId = args.Properties.MediaId;

        if (!string.IsNullOrEmpty(mediaId))
        {
          string json = JsonConvert.SerializeObject(args.PlaybackEvents);

          string script = string.Format("PlayerEventsListener.prototype.playbackEvents['{0}'] = {1};", mediaId, json);

          args.Result.BottomScripts.Add("mf_pe_" + args.MediaItem.ID, script);
        }
      }
    }
  }
}