using DevTrack.Infrastructure.BusinessObjects;

namespace DevTrack.Infrastructure.Services
{
    public interface IPasswordService
    {
        Task ChangePassword(UpdatePassword passwordUser);
    }
}