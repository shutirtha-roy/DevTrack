using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ApplicationUserEO = DevTrack.Infrastructure.Entities.ApplicationUser;

namespace DevTrack.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<ApplicationUserEO> _signInManager;
        private readonly UserManager<ApplicationUserEO> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountService(SignInManager<ApplicationUserEO> signInManager,
            UserManager<ApplicationUserEO> userManager,
            ITokenService tokenService,
            IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<(bool isValidUser, UserInfo? userInfo)> GetUserToken(string email, string password)
        {
            var applicationUser = await _userManager.FindByEmailAsync(email);

            if (applicationUser != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(applicationUser, password, true);

                if (result != null && result.Succeeded)
                {
                    var claims = (await _userManager.GetClaimsAsync(applicationUser)).ToList();
                    claims.Add(new Claim(ClaimTypes.Sid, applicationUser.Id.ToString()));
                    var jwtToken = await _tokenService.GetJwtTokenWithExpireDate(claims);

                    UserInfo userBO = _mapper.Map<UserInfo>(applicationUser);
                    userBO.Token = jwtToken.token;
                    userBO.ExpireDate = jwtToken.expireDate;

                    return (true, userBO);
                }
            }

            return (false, null);
        }
    }
}