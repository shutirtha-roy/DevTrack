using DevTrack.Infrastructure.BusinessObjects;
using Microsoft.AspNetCore.Http;

namespace DevTrack.Infrastructure.Services
{
    public interface ISettingsService
    {
        Task<ApplicationUser> GetUserSettings(Guid userId);
        Task EditUserSettings(ApplicationUser userSettings, IFormFile? imageFile);
    }
}