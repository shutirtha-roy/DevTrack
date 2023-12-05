using Autofac;
using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Enum;
using DevTrack.Infrastructure.Services;
using DevTrack.Web.Models;

namespace DevTrack.Web.Areas.Admin.Models
{
    public class UserModel : BaseModel
    {
        public ApplicationUser ApplicationUser { get; set; }

        private IUserService _userService;
        private IProjectService _projectService;

        public UserModel() : base()
        {

        }

        public UserModel(IUserService userService, IMapper mapper, IProjectService projectService, IClaimsService claimService)
        {
            _userService = userService;
            _projectService = projectService;
        }

        public override void ResolveDependency(ILifetimeScope scope)
        {
            base.ResolveDependency(scope);
            _userService = _scope.Resolve<IUserService>();
            _projectService = _scope.Resolve<IProjectService>();
        }

        internal async Task<object?> GetAllApplicationUsers()
        {
            var data = await _userService.GetApplicationUsers();

            return data;
        }

        internal async Task SetData(Guid userId)
        {
            ApplicationUser = await _userService.GetUserDetails(userId);
        }

        internal async Task<object?> GetUserOwnerProjects(Guid userId)
        {
            var data = await _projectService.GetProjectsByUserIdAndRole(userId, ProjectRole.Owner);

            foreach (var individualProject in data)
            {
                individualProject.ProjectUsers = null;
            }

            return data;
        }

        internal async Task<object?> GetUserWorkerProjects(Guid userId)
        {
            var data = await _projectService.GetProjectsByUserIdAndRole(userId, ProjectRole.Worker);

            foreach (var individualProject in data)
            {
                individualProject.ProjectUsers = null;
            }

            return data;
        }
    }
}