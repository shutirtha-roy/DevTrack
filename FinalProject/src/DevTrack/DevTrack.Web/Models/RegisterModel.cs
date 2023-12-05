using Autofac;
using DevTrack.Infrastructure.Entities;
using DevTrack.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace DevTrack.Web.Models
{
    public class RegisterModel : BaseModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Name { get; set; }
        public string? Image { get; set; }
        public string? ReturnUrl { get; set; }
        public IList<AuthenticationScheme>? ExternalLogins { get; set; }
        public string? TimeZone { get; set; }

        private IClaimsService _claimsService;

        public override void ResolveDependency(ILifetimeScope scope)
        {
            base.ResolveDependency(scope);
            _claimsService = _scope.Resolve<IClaimsService>();
        }

        internal async Task<string> GetLocalTimeZone()
        {
            return await _timeService.GetLocalTimeZoneRegion();
        }

        internal async Task SetAllDefaultClaims(ApplicationUser user)
        {
            await _claimsService.SetAllDefaultClaims(user);
        }
    }
}