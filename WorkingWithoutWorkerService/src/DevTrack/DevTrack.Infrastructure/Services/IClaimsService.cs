using DevTrack.Infrastructure.Entities;
using System.Security.Claims;
using UserClaimBO = DevTrack.Infrastructure.BusinessObjects.UserClaim;

namespace DevTrack.Infrastructure.Services
{
    public interface IClaimsService
    {
        Task<Claim[]> GetAllDefaultClaims();
        Task SetAllDefaultClaims(ApplicationUser user);
        Task<IList<Claim>> GetAllUserClaims(Guid userId);
        Task<IList<UserClaimBO>> GetSelectedClaims(IList<Claim> existingClaim);
        Task RemoveClaimsFromUser(Guid userId);
        Task AddClaimsToUser(Guid userId, IList<UserClaimBO> claims);
    }
}