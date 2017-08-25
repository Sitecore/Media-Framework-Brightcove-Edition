namespace AgencyOasis.MediaFramework.Brightcove.Upload
{
    public interface IVideoUploadService<out TConfig> : IVideoUploadService where TConfig : VideoUploadServiceConfigBase
    {
        TConfig Config { get; }
    }
}