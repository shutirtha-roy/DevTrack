using Autofac;
using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Enum;
using DevTrack.Infrastructure.Services;
using DevTrack.Web.Models;
using System.Security.Claims;

namespace DevTrack.Web.Areas.App.Models
{
    public class ProjectViewModel : BaseModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public bool AllowScreenshot { get; set; }
        public bool AllowWebcam { get; set; }
        public bool AllowKeyboardHit { get; set; }
        public bool AllowMouseClick { get; set; }
        public bool AllowActiveWindow { get; set; }
        public bool AllowManualTimeEntry { get; set; }
        public bool AllowRunningProgram { get; set; }
        public int TimeInterval { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; }     
        public ProjectRole Role { get; set; }

        private IProjectService _projectService;
        private IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        private IInvitationService _invitationService;

        public ProjectViewModel() : base()
        {

        }

        public ProjectViewModel(IProjectService projectService, IMapper mapper,
           IHttpContextAccessor httpContextAccessor, IInvitationService invitationService)
        {
            _projectService = projectService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _invitationService= invitationService;
        }

        public override void ResolveDependency(ILifetimeScope scope)
        {
            base.ResolveDependency(scope);
            _projectService = _scope.Resolve<IProjectService>();
            _mapper = _scope.Resolve<IMapper>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
        }

        private async Task<Guid> GetLoggedInUserId()
        {
            return Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        internal async Task LoadData(Guid id)
        {
            var userId = await GetLoggedInUserId();
            var project = await _projectService.GetProjectDetails(id);

            if (project != null)
            {
                _mapper.Map(project, this);
                UserName = project.ProjectUsers.Select(x => x.ApplicationUser.Name).FirstOrDefault();
                Role = project.ProjectUsers.Where(x => x.ApplicationUserId == userId).Select(x => x.Role).FirstOrDefault();
            }
        }

        internal async Task ArchiveProject(Guid id)
        {
            await _projectService.ArchiveProject(id);
        }

        internal async Task<Invitation> GetUserInvitation(Guid id)
        {
            var userId = await GetLoggedInUserId();
            var projectId = id;
            var invitation = await _invitationService.GetUserInvitation(userId, projectId);
            var invitationBO = _mapper.Map<Invitation>(invitation);

            return invitationBO;
        }
    }
}