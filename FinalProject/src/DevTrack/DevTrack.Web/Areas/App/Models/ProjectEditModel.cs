using Autofac;
using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Services;
using DevTrack.Web.Areas.App.Data;
using DevTrack.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace DevTrack.Web.Areas.App.Models
{
    public class ProjectEditModel : BaseModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Enter project title")]
        [Display(Name = "Project Title")]
        public string? Title { get; set; }
        public bool AllowScreenshot { get; set; }
        public bool AllowWebcam { get; set; }
        public bool AllowKeyboardHit { get; set; }
        public bool AllowMouseClick { get; set; }
        public bool AllowActiveWindow { get; set; }
        public bool AllowManualTimeEntry { get; set; }
        public bool AllowRunningProgram { get; set; }
        public int TimeInterval { get; set; }
        public bool IsArchived { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<SelectListItem> TimeIntervalList { get; set; } = TimeIntervalData.GetTimeIntervals();

        private IProjectService _projectService;
        private IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        private ITimeService _timeService;

        public ProjectEditModel() : base()
        {

        }

        public ProjectEditModel(IProjectService projectService, IMapper mapper,
            IHttpContextAccessor httpContextAccessor, ITimeService timeService)
        {
            _projectService = projectService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _timeService = timeService;
        }

        private async Task<Guid> GetLoggedInUserId()
        {
            return new Guid(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public override void ResolveDependency(ILifetimeScope scope)
        {
            base.ResolveDependency(scope);
            _projectService = _scope.Resolve<IProjectService>();
            _mapper = _scope.Resolve<IMapper>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
            _timeService = _scope.Resolve<ITimeService>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
        }

        internal async Task LoadData(Guid id)
        {
            var userId = await GetLoggedInUserId();
            var project = await _projectService.GetProject(id, userId);

            if (project != null)
            {
                _mapper.Map(project, this);
            }
        }

        internal async Task EditProject()
        {
            var project = _mapper.Map<Project>(this);
            var userId = await GetLoggedInUserId();

            await _projectService.EditProject(project, userId);
        }
    }
}
