using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using ApplicationUserEO = DevTrack.Infrastructure.Entities.ApplicationUser;

namespace DevTrack.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly ApplicationSignInManager _applicationSignInManager;

        public PasswordService(ApplicationUserManager applicationUserManager, ApplicationSignInManager applicationSignInManager)
        {
            _applicationUserManager = applicationUserManager;
            _applicationSignInManager = applicationSignInManager;
        }

        public async Task ChangePassword(UpdatePassword passwordUser)
        {
            var user = await _applicationUserManager.FindByIdAsync(passwordUser.Id.ToString());

            if (user == null)
                throw new Exception("User could not be found");

            var changePasswordResult = await _applicationUserManager.ChangePasswordAsync(user, passwordUser.OldPassword, passwordUser.NewPassword);

            if (!changePasswordResult.Succeeded)
                throw new Exception("Your password couldn't be changed");

            await _applicationSignInManager.RefreshSignInAsync(user);
        }
    }
}