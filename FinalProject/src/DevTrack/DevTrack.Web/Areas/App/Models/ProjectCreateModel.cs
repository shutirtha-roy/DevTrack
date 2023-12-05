using Autofac;
using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Enum;
using DevTrack.Infrastructure.Services;
using DevTrack.Web.Areas.App.Data;
using DevTrack.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace DevTrack.Web.Areas.App.Models
{
    public class ProjectCreateModel : BaseModel
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

        public ProjectCreateModel() : base()
        {

        }

        public ProjectCreateModel(IProjectService projectService, IMapper mapper,
            IHttpContextAccessor httpContextAccessor, ITimeService timeService)
        {
            _projectService = projectService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _timeService = timeService;
        }

        private void SetCreateDateTime()
        {
            CreatedDate = _timeService.Now;
        }

        public override void ResolveDependency(ILifetimeScope scope)
        {
            base.ResolveDependency(scope);
            _projectService = _scope.Resolve<IProjectService>();
            _mapper = _scope.Resolve<IMapper>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
            _timeService = _scope.Resolve<ITimeService>();
        }

        internal async Task CreateProject()
        {
            SetCreateDateTime();

            var project = _mapper.Map<Project>(this);

            var userId = new Guid(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            project.ProjectUsers = new List<ProjectUser>
            {
                new ProjectUser
                {
                    ProjectId = project.Id,
                    ApplicationUserId = userId,
                    Role = ProjectRole.Owner
                }
            };

            await _projectService.CreateProject(project);
        }
    }
}