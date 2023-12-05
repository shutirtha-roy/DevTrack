namespace DevTrack.Infrastructure.Services
{
    public interface IPathService
    {
        string RootPath { get; }
        string SettingsUserImageUploadLocation { get; }
        string ImageUploadLocation { get; }
        string ScreenCaptureLocation { get; }
        string WebcamImageLocation { get; }
    }
}