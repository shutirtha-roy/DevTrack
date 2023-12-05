using Autofac;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Enum;
using DevTrack.Infrastructure.Services;
using DevTrack.Web.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace DevTrack.Web.Areas.App.Models
{
    public class ActivityProjectUserModel : BaseModel
    {
        public Guid WorkerUserId { get; set; }
        public Guid OwnerUserId { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [ValidateNever]
        public IList<SelectListItem> ProjectList { get; set; }

        [ValidateNever]
        public IList<SelectListItem> UserList { get; set; }

        [ValidateNever]
        public Dictionary<DateOnly, Dictionary<(TimeOnly startTime, TimeOnly endTime), (string hourSlot, IList<Activity> reportSlot)?>> ReportActivityData { get; set; }

        private IActivityService? _activityService;
        private IProjectService? _projectService;
        private IHttpContextAccessor _httpContextAccessor;

        public ActivityProjectUserModel() : base()
        {

        }

        public ActivityProjectUserModel(IActivityService activityService, IHttpContextAccessor httpContextAccessor,
            IProjectService projectService)
        {
            _activityService = activityService;
            _projectService = projectService;
            _httpContextAccessor = httpContextAccessor;
        }

        internal async Task<Guid> GetLoggedInUserId()
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return userId;
        }

        internal async Task SetUserProjects(Guid userId, ProjectRole role)
        {
            IList<Project> projects = await _projectService.GetProjectsByUserIdAndRole(userId, role);

            ProjectList = new List<SelectListItem>();

            foreach (var project in projects)
            {
                ProjectList.Add(new SelectListItem { Value = $"{project.Id}", Text = project.Title });
            }
        }

        internal async Task SetUserProjectsWhenWorker(Guid userId)
        {
            await SetUserProjects(userId, ProjectRole.Worker);
        }

        internal async Task SetUserProjectsWhenOwner(Guid userId)
        {
            await SetUserProjects(userId, ProjectRole.Owner);
        }

        internal async Task SetProjectUsers(Guid projectId)
        {
            var users = await _projectService.GetUsersByProjectIdAndRole(projectId, ProjectRole.Worker);

            UserList = new List<SelectListItem>();

            foreach (var user in users)
            {
                UserList.Add(new SelectListItem { Value = $"{user.Id}", Text = user.Name });
            }
        }

        public override void ResolveDependency(ILifetimeScope scope)
        {
            base.ResolveDependency(scope);
            _activityService = _scope.Resolve<IActivityService>();
            _projectService = _scope.Resolve<IProjectService>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
        }
    }
}