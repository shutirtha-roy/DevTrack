using DevTrack.Infrastructure.BusinessObjects;

namespace DevTrack.Infrastructure.Services
{
    public interface IAccountService
    {
        Task<(bool isValidUser, UserInfo userInfo)> GetUserToken(string email, string password);
    }
}