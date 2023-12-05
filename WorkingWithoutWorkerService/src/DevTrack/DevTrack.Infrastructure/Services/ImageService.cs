using DevTrack.Infrastructure.Enum;
﻿using DevTrack.Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace DevTrack.Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly IPathService _pathService;

        public ImageService(IPathService pathService)
        {
            _pathService = pathService;
        }

        public async Task StoreUploadedImage(IFormFile? imageFile, string imageLocation, string imageNameWithExtension)
        {
            await ResizeImage(imageFile, imageLocation, imageNameWithExtension);
        }

        public async Task ResizeImage(IFormFile? imageFile, string imageLocation, string imageNameWithExtension)
        {
            using var image = Image.Load(imageFile.OpenReadStream());

            image.Mutate(x => x.Resize(256, 256));

            image.Save(Path.Combine(imageLocation, imageNameWithExtension));
        }

        public async Task DeletePreviousUplodedImage(string oldImagePath)
        {
            if (File.Exists(oldImagePath))
            {
                File.Delete(oldImagePath);
            }
        }

        private async Task CreateDirectoryIfMissing(string location)
        {
            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }
        }

        private async Task<string> GetImageFileName(ApplicationUser user)
        {
            return $"{Guid.NewGuid()}_{user.Name}".Trim().Replace(' ', '_');
        }

        public async Task<string> GetUploadedImage(ApplicationUser user, IFormFile? imageFile)
        {
            string fileName = await GetImageFileName(user);
            var uploads = Path.Combine(_pathService.RootPath, _pathService.SettingsUserImageUploadLocation);
            var extension = Path.GetExtension(imageFile.FileName);
            string ImageName = user.Image;

            if (ImageName != null)
            {
                var oldImagePath = Path.Combine(uploads, ImageName);

                try
                {
                    await DeletePreviousUplodedImage(oldImagePath);
                }
                catch (Exception)
                {
                    throw new Exception("Profile Image Replacing Is Not Possible");
                }
            }

            await CreateDirectoryIfMissing(uploads);

            try
            {
                await StoreUploadedImage(imageFile, uploads, fileName + extension);
            }
            catch (Exception ex)
            {
                throw new Exception("Profile Image Upload Is Not Possible");
            }

            string newImageNameWithExtension = fileName + extension;

            return newImageNameWithExtension;
        }

        public async Task<string> ConvertBase64StringToImage(string imagesBinary, ImageType type)
        {
            var fileNameWithExtension = $"{Guid.NewGuid()}.png";

            var filePath = "";
            if (type == ImageType.WebcamCapture)
            {
                await CreateDirectoryIfMissing(_pathService.WebcamImageLocation);
                filePath = $"{_pathService.WebcamImageLocation}\\{fileNameWithExtension}";
            }
            else
            {
                await CreateDirectoryIfMissing(_pathService.ScreenCaptureLocation);
                filePath = $"{_pathService.ScreenCaptureLocation}\\{fileNameWithExtension}";
            }

            File.WriteAllBytes(filePath, Convert.FromBase64String(imagesBinary));

            return fileNameWithExtension;
        }
    }
}