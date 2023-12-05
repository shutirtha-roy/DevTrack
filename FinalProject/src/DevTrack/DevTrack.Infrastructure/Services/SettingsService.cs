using AutoMapper;
using DevTrack.Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using ApplicationUserBO = DevTrack.Infrastructure.BusinessObjects.ApplicationUser;
using ApplicationUserEO = DevTrack.Infrastructure.Entities.ApplicationUser;

namespace DevTrack.Infrastructure.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly ApplicationUserManager _applicationUserManager;
        
        public SettingsService(IMapper mapper, IApplicationUnitOfWork applicationUnitOfWork, IImageService imageService, ApplicationUserManager applicationUserManager)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
            _imageService = imageService;
            _applicationUserManager = applicationUserManager;
        }

        public async Task<ApplicationUserBO> GetUserSettings(Guid userId)
        {
            ApplicationUserEO userSettingsData = _applicationUnitOfWork.Settings.GetById(userId);

            ApplicationUserBO userProfile = _mapper.Map<ApplicationUserBO>(userSettingsData);

            return userProfile;
        }

        private async Task<string> SetCorrectEmail(string currentEmail, string userProfileEmailAfterUpdate)
        {
            if (currentEmail != userProfileEmailAfterUpdate)
            {
                return currentEmail;
            }
            else
            {
                return userProfileEmailAfterUpdate;
            }
        }

        public async Task EditUserSettings(ApplicationUserBO userSettings, IFormFile? imageFile)
        {
            if(userSettings == null)
            {
                throw new InvalidOperationException("User settings was not found");
            }

            ApplicationUserEO userProfile = await _applicationUserManager.FindByIdAsync(userSettings.Id.ToString());
            var currentEmail = userProfile.Email;

            userProfile = _mapper.Map(userSettings, userProfile);

            userProfile.Email = await SetCorrectEmail(currentEmail, userProfile.Email);

            if (imageFile != null)
            {
                userProfile.Image = await _imageService.GetUploadedImage(userProfile, imageFile);
            }
            
            await _applicationUserManager.UpdateAsync(userProfile);
        }
    }
}