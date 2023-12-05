using Autofac;
using AutoMapper;
using DevTrack.Infrastructure.Services;
using DevTrack.Web.Models;
using System.Security.Claims;

namespace DevTrack.Web.Areas.App.Models
{
    public class UserProjectsModel : BaseModel
    {
        public Guid UserId { get; set; }
        public IList<string> OwnerProjectTitles { get; set; }
        public IList<string> WorkerProjectTitles { get; set; }
        public IList<double> LogHoursPerProjectOwner { get; set; }
        public IList<double> LogHoursPerProjectWorker{ get; set; }
        public IList<double> LogHoursPerMonthOwner { get; set; }
        public IList<double> LogHoursPerMonthWorker { get; set; }

        private IProjectService _projectService;
        private IHttpContextAccessor _httpContextAccessor;

        public UserProjectsModel() : base()
        {

        }

        public UserProjectsModel(IProjectService projectService, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _projectService = projectService;
            _httpContextAccessor = httpContextAccessor;
        }

        public override void ResolveDependency(ILifetimeScope scope)
        {
            base.ResolveDependency(scope);
            _projectService = _scope.Resolve<IProjectService>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
        }

        public async Task GetLoggedHoursPerProject()
        {
            UserId = new Guid(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            (OwnerProjectTitles, WorkerProjectTitles, LogHoursPerProjectOwner, LogHoursPerProjectWorker, LogHoursPerMonthOwner, LogHoursPerMonthWorker) = await _projectService.GetLoggedHoursPerProject(UserId);
        }
    }
}