using AutoMapper;
using DevTrack.Infrastructure.Enum;
using DevTrack.Infrastructure.Exceptions;
using DevTrack.Infrastructure.UnitOfWorks;
using ProjectBO = DevTrack.Infrastructure.BusinessObjects.Project;
using ProjectEO = DevTrack.Infrastructure.Entities.Project;
using ProjectSummaryBO = DevTrack.Infrastructure.BusinessObjects.ProjectSummary;
using TeamMemberBO = DevTrack.Infrastructure.BusinessObjects.TeamMember;
using ApplicationUserBO = DevTrack.Infrastructure.BusinessObjects.ApplicationUser;

namespace DevTrack.Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ITimeService _timeService;

        public ProjectService(IMapper mapper, IApplicationUnitOfWork applicationUnitOfWork, ITimeService timeService)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
            _timeService = timeService;
        }

        public async Task CreateProject(ProjectBO project)
        {
            var userId = project.ProjectUsers?.Select(x => x.ApplicationUserId).FirstOrDefault();

            var count = _applicationUnitOfWork.Projects.GetCount(x => x.Title == project.Title && x.ProjectUsers.Any(x => x.ApplicationUserId == userId));

            if (count > 0)
                throw new DuplicateException("Project title already exists.");

            var projectEntity = _mapper.Map<ProjectEO>(project);

            _applicationUnitOfWork.Projects.Add(projectEntity);
            _applicationUnitOfWork.Save();
        }

        public async Task<ProjectBO> GetProject(Guid projectId, Guid userId)
        {
            var projectEO = _applicationUnitOfWork.Projects.GetById(projectId);

            var countOwner = _applicationUnitOfWork.Projects.GetCount(x => x.Id == projectEO.Id && x.ProjectUsers.Any(x => x.ApplicationUserId == userId && x.Role == ProjectRole.Owner));

            if (countOwner == 0)
                throw new AuthorizationException("You are not the authorized owner.");

            ProjectBO projectBO = _mapper.Map<ProjectBO>(projectEO);

            return projectBO;
        }

        public async Task EditProject(ProjectBO project, Guid userId)
        {
            var projectEO = _applicationUnitOfWork.Projects.GetProjectDetailsById(project.Id);
            var projectUser = projectEO.ProjectUsers;

            var countOwner = _applicationUnitOfWork.Projects.GetCount(x => x.Id == projectEO.Id && x.ProjectUsers.Any(x => x.ApplicationUserId == userId && x.Role == ProjectRole.Owner));

            if (countOwner == 0)
                throw new AuthorizationException("You are not the authorized owner.");

            var countProjects = _applicationUnitOfWork.Projects.GetCount(x => x.Title == project.Title && x.ProjectUsers.Any(x => x.ApplicationUserId == userId));

            if (countProjects > 0 && projectEO.Title != project.Title)
                throw new DuplicateException("Project title already exists.");

            projectEO = _mapper.Map(project, projectEO);
            projectEO.ProjectUsers = projectUser;

            _applicationUnitOfWork.Save();
        }

        public async Task<ProjectBO> GetProjectDetails(Guid projectId)
        {
            var projectEO = _applicationUnitOfWork.Projects.GetProjectDetailsById(projectId);

            ProjectBO projectBO = _mapper.Map<ProjectBO>(projectEO);

            return projectBO;
        }

        public async Task<IList<ProjectBO>> GetProjectsByUserIdAndRole(Guid userId, ProjectRole role)
        {
            var projects = _applicationUnitOfWork.Projects.GetProjectsByUserAndRole(userId, role); 
            var allWorkerUserProjects = new List<ProjectBO>();

            foreach (var projectEO in projects)
            {
                ProjectBO projectBO = _mapper.Map<ProjectBO>(projectEO);
                allWorkerUserProjects.Add(projectBO);
            }

            return allWorkerUserProjects;
        }

        public async Task<(int total, int totalDisplay, IList<ProjectSummaryBO> records)> GetProjects(
            int pageIndex, int pageSize, string searchText, Guid applicationUserId, string title, bool? isArchived, int? role, string orderby)
        {
            (IList<ProjectSummaryBO> data, int total, int totalDisplay) results = await _applicationUnitOfWork
                .Projects.GetProjects(pageIndex, pageSize, searchText, applicationUserId, title, isArchived, role, orderby);

            return (results.total, results.totalDisplay, results.data);
        }

        public async Task<(int total, int totalDisplay, IList<TeamMemberBO> records)> GetTeamMembers(
            int pageIndex, int pageSize, string searchText, Guid projectId, Guid applicationUserId, string orderby)
        {
            (IList<TeamMemberBO> data, int total, int totalDisplay) results = await _applicationUnitOfWork
                .Projects.GetTeamMembers(pageIndex, pageSize, searchText, projectId, applicationUserId, _timeService.Now.Date, orderby);

            return (results.total, results.totalDisplay, results.data);
        }
        public async Task ArchiveProject(Guid id)
        {
            var project = _applicationUnitOfWork.Projects.GetById(id);
            project.IsArchived= true;
            _applicationUnitOfWork.Save();
        }

        public async Task<IList<ApplicationUserBO>> GetUsersByProjectIdAndRole(Guid projectId, ProjectRole role)
        {
            var users = _applicationUnitOfWork.Projects.GetUsersByProjectIdAndRole(projectId, role);

            IList<ApplicationUserBO> applicationUsers = new List<ApplicationUserBO>();

            foreach(var user in users)
            {
                ApplicationUserBO userBO = _mapper.Map<ApplicationUserBO>(user);
                applicationUsers.Add(userBO);
            }

            return applicationUsers;
        }
    }
}