using ApplicationUserBO = DevTrack.Infrastructure.BusinessObjects.ApplicationUser;

namespace DevTrack.Infrastructure.Services
{
    public interface IUserService
    {
        Task<IList<ApplicationUserBO>> GetApplicationUsers();
        Task<ApplicationUserBO> GetUserDetails(Guid userId);
    }
}