using Autofac;
using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Services;
using DevTrack.Web.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace DevTrack.Web.Areas.App.Models
{
    public class PasswordEditModel : BaseModel
    {
        public Guid Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        private IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        private ISettingsService _settingsService;
        private IPathService _pathService;
        private IPasswordService _passwordService;

        public PasswordEditModel()
        {

        }

        public PasswordEditModel(ISettingsService settingsService, IMapper mapper, IHttpContextAccessor httpContextAccessor, 
            IPathService pathService, IPasswordService passwordService)
        {
            _settingsService = settingsService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _pathService = pathService;
            _passwordService = passwordService;
        }

        private async Task<Guid> GetLoggedInUserId()
        {
            var userId = new Guid(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return userId;
        }

        public override void ResolveDependency(ILifetimeScope scope)
        {
            base.ResolveDependency(scope);
            _mapper = _scope.Resolve<IMapper>();
            _settingsService = _scope.Resolve<ISettingsService>();
            _pathService = _scope.Resolve<IPathService>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
            _passwordService = _scope.Resolve<IPasswordService>();
        }

        public async Task ChangePassword()
        {
            Id = await GetLoggedInUserId();
            var passwordUser = _mapper.Map<UpdatePassword>(this);
            await _passwordService.ChangePassword(passwordUser);
        }
    }
}