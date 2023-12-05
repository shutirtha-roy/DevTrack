using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace DevTrack.Infrastructure.Services
{
    public class LocalPathService : IPathService
    {
        public string RootPath { get; private set; }
        public string SettingsUserImageUploadLocation { get; private set; }
        public string ImageUploadLocation { get; private set; }
        public string ScreenCaptureLocation { get; private set; }
        public string WebcamImageLocation { get; private set; }

        public LocalPathService(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            SettingsUserImageUploadLocation = configuration.GetValue<string>("SettingsUserImageUploadLocation");
            ScreenCaptureLocation = configuration.GetValue<string>("ScreeCaptureImageLocation");
            WebcamImageLocation = configuration.GetValue<string>("WebcamCaptureImageLocation");
            RootPath = hostEnvironment.WebRootPath;
            ImageUploadLocation = $"/{SettingsUserImageUploadLocation.Replace('\\', '/')}/";
        }
    }
}