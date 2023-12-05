using Autofac;
using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Services;
using DevTrack.Web.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace DevTrack.Web.Areas.App.Models
{
    public class SettingsEditModel : BaseModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }
        public string? Image { get; set; }
        public string? ImageLocation { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? TimeZone { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> TimeZoneList { get; set; }

        private IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        private ISettingsService _settingsService;
        private IPathService _pathService;

        public SettingsEditModel()
        {

        }

        public SettingsEditModel(ISettingsService settingsService, IMapper mapper, IHttpContextAccessor httpContextAccessor, 
            IPathService pathService, ITimeService timeService)
        {
            _settingsService = settingsService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _pathService = pathService;
            _timeService = timeService;
        }

        public override void ResolveDependency(ILifetimeScope scope)
        {
            base.ResolveDependency(scope);
            _mapper = _scope.Resolve<IMapper>();
            _settingsService = _scope.Resolve<ISettingsService>();
            _pathService = _scope.Resolve<IPathService>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
        }

        internal async Task EditSettings()
        {
            ApplicationUser data = _mapper.Map<ApplicationUser>(this);
            await _settingsService.EditUserSettings(data, ImageFile);
        }

        private async Task SetLocalImageLocation()
        {
            ImageLocation = _pathService.ImageUploadLocation + Image;
        }

        internal async Task SetAllTimeZones()
        {
            TimeZoneList = await _timeService.GetAllTimeZone();
        }

        private async Task<Guid> GetLoggedInUserId(IHttpContextAccessor httpContextAccessor)
        {
            var userId = new Guid(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return userId;
        }

        public async Task<SettingsEditModel> GetProfileData()
        {
            var userId = await GetLoggedInUserId(_httpContextAccessor);
            ApplicationUser data = await _settingsService.GetUserSettings(userId);

            if (data != null)
            {
                _mapper.Map(data, this);
            }

            await SetLocalImageLocation();
            await SetAllTimeZones();

            return this;
        }
    }
}