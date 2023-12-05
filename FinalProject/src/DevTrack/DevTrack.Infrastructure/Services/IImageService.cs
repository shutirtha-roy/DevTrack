using DevTrack.Infrastructure.Enum;
﻿using DevTrack.Infrastructure.Entities;
using Microsoft.AspNetCore.Http;

namespace DevTrack.Infrastructure.Services
{
    public interface IImageService
    {
        Task StoreUploadedImage(IFormFile? imageFile, string imageLocation, string imageNameWithExtension);
        Task DeletePreviousUplodedImage(string oldImagePath);
        Task<string> GetUploadedImage(ApplicationUser user, IFormFile? imageFile);
        Task<string> ConvertBase64StringToImage(string imagesBinary, ImageType type);
        Task ResizeImage(IFormFile? imageFile, string imageLocation, string imageNameWithExtension);
    }
}