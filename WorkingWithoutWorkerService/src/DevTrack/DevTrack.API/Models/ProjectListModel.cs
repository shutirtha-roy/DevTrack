using Autofac;
using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Enum;
using DevTrack.Infrastructure.Services;
using System.Net;

namespace DevTrack.API.Models
{
    public class ProjectListModel : BaseModel
    {
        public IList<ProjectModel> ProjectModels { get; private set; }

        private IProjectService _projectService;
        private IMapper _mapper;

        public ProjectListModel() : base()
        {

        }

        public ProjectListModel(IProjectService projectService, IMapper mapper,
           IHttpContextAccessor httpContextAccessor)
        {
            _projectService = projectService;
            _mapper = mapper;
            ProjectModels = new List<ProjectModel>();
        }

        public override void ResolveDependency(ILifetimeScope scope)
        {
            base.ResolveDependency(scope);
            _projectService = _scope.Resolve<IProjectService>();
            _mapper = _scope.Resolve<IMapper>();
        }

        private async Task AddProjectsToList(IList<Project> projects, ProjectRole role)
        {
            foreach (var project in projects)
            {
                ProjectModel projectModel = new ProjectModel();
                projectModel = _mapper.Map(project, projectModel);
                projectModel.Role = (int)role;

                ProjectModels.Add(projectModel);
            }
        }

        internal async Task<ProjectResponseModel> GetProjectsByUserIdAndWorker(Guid userId)
        {
            var projects = await _projectService.GetProjectsByUserIdAndRole(userId, ProjectRole.Worker);

            await AddProjectsToList(projects, ProjectRole.Worker);

            var model = new ProjectResponseModel();

            if(projects != null)
            {
                model.IsSuccess = true;
                model.StatusCode = (int)HttpStatusCode.OK;
                model.Data = ProjectModels;
                model.Errors = new string[] { };
            }
            else
            {
                model.IsSuccess = false;
                model.StatusCode = (int)HttpStatusCode.BadRequest;
                model.Data = null;
                model.Errors = new string[] { "We are unable to fetch your projects" };
            }

            return model;
        }
    }
}