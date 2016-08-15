

namespace Sitecore.MediaFramework.Brightcove.Export
{
  using System.Collections.Generic;
  using Sitecore.Data.Items;
  using Sitecore.MediaFramework.Brightcove.Entities;
  using Sitecore.MediaFramework.Brightcove.Security;
  using Sitecore.MediaFramework.Export;
  using Sitecore.RestSharp;

  using global::RestSharp;

  public class PlaylistExporter : ExportExecuterBase
  {
    protected override object Create(ExportOperation operation)
    {
      var synchronizer = MediaFrameworkContext.GetItemSynchronizer(operation.Item);
      if (synchronizer == null)
      {
        return null;
      }

      var playlist = (PlayList)synchronizer.CreateEntity(operation.Item);

      playlist.Id = null;
      playlist.ReferenceId = null;
      playlist.ThumbnailUrl = null;

      //Video ids used only for EXPLICIT playlist.
      //In other case will be used filter tags & tag inclusion
      if (playlist.PlaylistType == PlaylistType.EXPLICIT.ToString())
      {
        playlist.FilterTags = null;
        playlist.TagInclusion = null;
      }
      else
      {
        playlist.VideoIds = null;
      }

      var authenticator = new BrightcoveAthenticator(operation.AccountItem);

      var updateData = new PostData("create_playlist", authenticator, "playlist", playlist);

      var context = new RestContext(Constants.SitecoreRestSharpService, authenticator);

      var data = context.Create<PostData, ResultData<string>>("update_data", updateData).Data;


      if (data != null && !string.IsNullOrEmpty(data.Result))
      {
        //we could not to use existing playlist object because it does not contain all data
        var playlistData = context.Read<PlayList>("read_playlist_by_id",
            new List<Parameter>
              {
                new Parameter { Type = ParameterType.UrlSegment, Name = "playlist_id", Value = data.Result }
              }).Data;

        if (playlistData == null)
        {
          playlist.Id = data.Result;
          return playlist;         
        }

        return playlistData;
      }

      return null;
    }

    protected override void Delete(ExportOperation operation)
    {
      var synchronizer = MediaFrameworkContext.GetItemSynchronizer(operation.Item);
      if (synchronizer == null)
      {
        return;
      }

      var playlist = (PlayList)synchronizer.CreateEntity(operation.Item);

      var parameters = new Dictionary<string, object>
        {
          { "playlist_id", playlist.Id },
          { "cascade", "true" },
        };

      var authenticator = new BrightcoveAthenticator(operation.AccountItem);
      
      var updateData = new PostData("delete_playlist", authenticator, parameters);

      var context = new RestContext(Constants.SitecoreRestSharpService, authenticator);

      context.Delete<PostData, ResultData<PlayList>>("update_data", updateData);
    }

    protected override object Update(ExportOperation operation)
    {
      var synchronizer = MediaFrameworkContext.GetItemSynchronizer(operation.Item);
      if (synchronizer == null)
      {
        return null;
      }

      var playlist = (PlayList)synchronizer.CreateEntity(operation.Item);

      playlist.ReferenceId = null;
      playlist.ThumbnailUrl = null;

      //Video ids used only for EXPLICIT playlist.
      //In other case will be used filter tags & tag inclusion
      if (playlist.PlaylistType == PlaylistType.EXPLICIT.ToString())
      {
        playlist.FilterTags = null;
        playlist.TagInclusion = null;
      }
      else
      {
        playlist.VideoIds = null;
      }

      var authenticator = new BrightcoveAthenticator(operation.AccountItem);

      var updateData = new PostData("update_playlist", authenticator,"playlist", playlist);

      var context = new RestContext(Constants.SitecoreRestSharpService, authenticator);

      var data = context.Update<PostData, ResultData<PlayList>>("update_data", updateData).Data;

      return data != null ? data.Result : null;
    }

    public override bool IsNew(Item item)
    {
      return item[FieldIDs.MediaElement.Id].Length == 0;
    }
  }
}