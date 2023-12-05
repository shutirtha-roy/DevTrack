using DevTrack.Infrastructure.Entities;
using System.Security.Claims;
using UserClaimBO = DevTrack.Infrastructure.BusinessObjects.UserClaim;

namespace DevTrack.Infrastructure.Services
{
    public class ClaimsService : IClaimsService
    {
        private readonly ApplicationUserManager _applicationUserManager;
        private const string _createClaim = "CreateProject";
        private const string _editClaim = "EditProject";
        private const string _viewClaim = "ViewProject";
        private const string _deleteClaim = "DeleteProject";
        private const string _archiveClaim = "ArchiveProject";
        private const string _allowClaim = "true";

        public ClaimsService(ApplicationUserManager applicationUserManager)
        {
            _applicationUserManager = applicationUserManager;
        }

        public async Task<Claim[]> GetAllDefaultClaims()
        {
            return new Claim[]
            {
                new Claim(_createClaim, _allowClaim), new Claim(_editClaim,_allowClaim),
                new Claim(_viewClaim, _allowClaim), new Claim(_deleteClaim,_allowClaim),
                new Claim(_archiveClaim, _allowClaim)
            };
        }

        public async Task SetAllDefaultClaims(ApplicationUser user)
        {
            await _applicationUserManager.AddClaimsAsync(user, await GetAllDefaultClaims());
        }

        public async Task<IList<Claim>> GetAllUserClaims(Guid userId)
        {
            var user = await _applicationUserManager.FindByIdAsync(userId.ToString());

            var userClaims = await _applicationUserManager.GetClaimsAsync(user);

            return userClaims;
        }

        public async Task<IList<UserClaimBO>> GetSelectedClaims(IList<Claim> existingClaim)
        {
            var allClaims = await GetAllDefaultClaims();
            var userSelectedClaims = new List<UserClaimBO>();

            foreach(var claim in allClaims)
            {
                var userClaim = new UserClaimBO
                {
                    ClaimType = claim.Type
                };

                if (existingClaim.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }

                userSelectedClaims.Add(userClaim);
            }

            return userSelectedClaims;
        }

        public async Task RemoveClaimsFromUser(Guid userId)
        {
            var user = await _applicationUserManager.FindByIdAsync(userId.ToString());

            var claims = await _applicationUserManager.GetClaimsAsync(user);

            await _applicationUserManager.RemoveClaimsAsync(user, claims);
        }

        public async Task AddClaimsToUser(Guid userId, IList<UserClaimBO> claims)
        {
            var user = await _applicationUserManager.FindByIdAsync(userId.ToString());

            var result = await _applicationUserManager.AddClaimsAsync(user, 
                claims.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.ClaimType)));

            if(!result.Succeeded)
            {
                throw new Exception("Cannot add selected claims to user");
            }
        }
    }
}