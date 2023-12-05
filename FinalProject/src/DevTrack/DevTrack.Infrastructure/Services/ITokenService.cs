using System.Security.Claims;

namespace DevTrack.Infrastructure.Services
{
    public interface ITokenService
    {
        Task<string> GetJwtToken(IList<Claim> claims);
        Task<(string token, DateTime expireDate)> GetJwtTokenWithExpireDate(IList<Claim> claims);
    }
}