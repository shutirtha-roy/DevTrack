using AutoMapper;
using DevTrack.Infrastructure.Enum;
using DevTrack.Infrastructure.Exceptions;
using DevTrack.Infrastructure.UnitOfWorks;
using ProjectBO = DevTrack.Infrastructure.BusinessObjects.Project;
using ProjectEO = DevTrack.Infrastructure.Entities.Project;
using ProjectSummaryBO = DevTrack.Infrastructure.BusinessObjects.ProjectSummary;
using TeamMemberBO = DevTrack.Infrastructure.BusinessObjects.TeamMember;
using ApplicationUserBO = DevTrack.Infrastructure.BusinessObjects.ApplicationUser;
using DevTrack.Infrastructure.Entities;

namespace DevTrack.Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ITimeService _timeService;
        private readonly ApplicationUserManager _applicationUserManager;

        public ProjectService(IMapper mapper, IApplicationUnitOfWork applicationUnitOfWork, ITimeService timeService
            , ApplicationUserManager applicationUserManager)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
            _timeService = timeService;
            _applicationUserManager = applicationUserManager;
        }

        public async Task CreateProject(ProjectBO project)
        {
            var userId = project.ProjectUsers?.Select(x => x.ApplicationUserId).FirstOrDefault();
            var user = await _applicationUserManager.FindByIdAsync(userId?.ToString());
            if (user == null)
                throw new InvalidOperationException("User not found");

            var count = _applicationUnitOfWork.Projects.GetCount(x => x.Title == project.Title && x.ProjectUsers.Any(x => x.ApplicationUserId == userId));

            if (count > 0)
                throw new DuplicateException("Project title already exists.");

            var projectEntity = _mapper.Map<ProjectEO>(project);
            projectEntity.Invitations = new List<Invitation>
            {
                new Invitation
                {
                    Email = user.Email,
                    Status = InvitationStatus.Accepted,
                    Date = _timeService.Now
                }
            };

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
            project.IsArchived = true;
            _applicationUnitOfWork.Save();
        }

        public async Task RestoreProject(Guid id)
        {
            var project = _applicationUnitOfWork.Projects.GetById(id);
            project.IsArchived = false;
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

        public async Task<(IList<string> ownerProjectTitles, IList<string> workerProjectTitles, IList<double> loggedHoursPerOwnerProject, IList<double> loggedHoursPerWorkerProject, IList<double> loggedHoursPerMonthOwner, IList<double> loggedHoursPerMonthWorker)> GetLoggedHoursPerProject(Guid userId)
        {
            IList<string> ownerProjectTitles = new List<string>();
            IList<string> workerProjectTitles = new List<string>();
            (ownerProjectTitles, workerProjectTitles) = _applicationUnitOfWork.Projects.GetUserProjectTitles(userId);

            var ownerProjects = _applicationUnitOfWork.Projects.GetProjectsByUserAndRole(userId, ProjectRole.Owner).ToList();
            var workerProjects = _applicationUnitOfWork.Projects.GetProjectsByUserAndRole(userId, ProjectRole.Worker).ToList();

            IList<double> loggedHoursPerOwnerProject = new List<double>();
            loggedHoursPerOwnerProject = await GetloggedHoursPerProject(ownerProjects);

            IList<double> loggedHoursPerWorkerProject = new List<double>();
            loggedHoursPerWorkerProject = await GetloggedHoursPerProject(workerProjects, userId);

            var ownerProjectIDs = ownerProjects.Select(x => x.Id).ToList();
            var workerProjectIDs = workerProjects.Select(x => x.Id).ToList();

            var currentYear = _timeService.Now.Year;
            var months = Enumerable.Range(1, 12).Select(x => x).ToList();

            var loggedHoursPerMonthOwner = await GetloggedHoursPerMonth(months, ownerProjectIDs, currentYear);
            var loggedHoursPerMonthWorker = await GetloggedHoursPerMonth(months, workerProjectIDs, currentYear,userId);

            return (ownerProjectTitles, workerProjectTitles, loggedHoursPerOwnerProject, loggedHoursPerWorkerProject, loggedHoursPerMonthOwner, loggedHoursPerMonthWorker);
        }

        public async Task<IList<double>> GetloggedHoursPerProject(IList<ProjectEO> projects)
        {
            var loggedHoursPerProject = new List<double>();
            foreach (var p in projects)
            {
                var projectLoggedTime = await _applicationUnitOfWork.Activities.GetprojectLoggedTime(p);
                var timeSpan = TimeSpan.FromTicks(projectLoggedTime);
                loggedHoursPerProject.Add(timeSpan.TotalHours);
            }

            return loggedHoursPerProject;
        }

        public async Task<IList<double>> GetloggedHoursPerProject(IList<ProjectEO> projects,Guid userId)
        {
            var loggedHoursPerProject = new List<double>();
            foreach (var p in projects)
            {
                long projectLoggedTime=0;
                var activity = _applicationUnitOfWork.Activities.Get(x => x.Project == p && x.ApplicationUserId == userId, "").ToList();
                projectLoggedTime = await _applicationUnitOfWork.Activities.GetprojectLoggedTime(p,userId);
                var timeSpan = TimeSpan.FromTicks(projectLoggedTime);
                loggedHoursPerProject.Add(timeSpan.TotalHours);
            }

            return loggedHoursPerProject;
        }

        public async Task<IList<double>> GetloggedHoursPerMonth(IList<int> months,IList<Guid> projectIds,int currentYear)
        {
            var loggedHoursPerMonth = new List<double>();
            foreach (var m in months)
            {
                var timeSpan = TimeSpan.Zero;
                long monthlyLoggedTime = 0;
                foreach (var pId in projectIds)
                {
                    monthlyLoggedTime += await _applicationUnitOfWork.Activities.GetmonthlyLoggedTime(m, currentYear, pId);
                    timeSpan = TimeSpan.FromTicks(monthlyLoggedTime); 
                }
                loggedHoursPerMonth.Add(timeSpan.TotalHours);
            }

            return loggedHoursPerMonth;
        }

        public async Task<IList<double>> GetloggedHoursPerMonth(IList<int> months, IList<Guid> projectIds, int currentYear, Guid userId)
        {
            var loggedHoursPerMonth = new List<double>();
            foreach (var m in months)
            {
                var timeSpan = TimeSpan.Zero;
                long monthlyLoggedTime = 0;
                foreach (var pId in projectIds)
                {
                    monthlyLoggedTime += await _applicationUnitOfWork.Activities.GetmonthlyLoggedTime(m, currentYear, userId, pId);
                    timeSpan = TimeSpan.FromTicks(monthlyLoggedTime);
                }
                loggedHoursPerMonth.Add(timeSpan.TotalHours);
            }

            return loggedHoursPerMonth;
        }
    }
}