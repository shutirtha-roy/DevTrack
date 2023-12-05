using DevTrack.Infrastructure.DbContexts;
using DevTrack.Infrastructure.Entities;
using DevTrack.Infrastructure.Enum;
using Microsoft.EntityFrameworkCore;
using ProjectSummaryBO = DevTrack.Infrastructure.BusinessObjects.ProjectSummary;
using TeamMemberBO = DevTrack.Infrastructure.BusinessObjects.TeamMember;

namespace DevTrack.Infrastructure.Repositories
{
    public class ProjectRepository : Repository<Project, Guid>, IProjectRepository
    {
        private readonly IApplicationDbContext _dbContext;

        public ProjectRepository(IApplicationDbContext context) : base((DbContext)context)
        {
            _dbContext = context;
        }

        public Project GetProjectDetailsById(Guid id)
        {
            return _dbContext.Projects.Include(x => x.ProjectUsers).ThenInclude(x => x.ApplicationUser).Where(y => y.Id == id).FirstOrDefault();
        }

        public IEnumerable<Project> GetProjectsByUserAndRole(Guid userId, ProjectRole role)
        {
            return _dbContext.Projects.Where(x => x.IsArchived == false).Include(x => x.ProjectUsers).ThenInclude(y => y.ApplicationUser)
                .Where(x => x.ProjectUsers.Any(x => x.ApplicationUserId == userId && x.Role.Equals(role)));
        }
        public IList<string> GetUserEmailsByProjectID(Guid projectId) 
        {
            var project = GetProjectDetailsById(projectId);
            return project.ProjectUsers.Select(x => x.ApplicationUser).Select(y => y.Email).ToList();
        }

        public async Task<(IList<ProjectSummaryBO> data, int total, int totalDisplay)> GetProjects(
            int pageIndex, int pageSize, string searchText, Guid applicationUserId, string title, bool? isArchived, int? role, string orderby)
        {
            var result = await QueryWithStoredProcedureAsync<ProjectSummaryBO>("sp_GetProjects", new Dictionary<string, object>
            {
                {"ApplicationUserId", applicationUserId},
                {"Title", title},
                {"IsArchived", isArchived},
                {"Role", role},
                {"PageIndex", pageIndex},
                {"PageSize", pageSize},
                {"SearchText", searchText},
                {"OrderBy", orderby }
            },
            new Dictionary<string, Type>
            {
                {"Total", typeof(int)},
                {"TotalDisplay", typeof(int)}
            });

            return (result.result, int.Parse(result.outValues.ElementAt(0).Value.ToString()), int.Parse(result.outValues.ElementAt(1).Value.ToString()));
        }

        public async Task<(IList<TeamMemberBO> data, int total, int totalDisplay)> GetTeamMembers(int pageIndex,
            int pageSize, string searchText, Guid projectId, Guid applicationUserId, DateTime currentDate, string orderby)
        {
            var result = await QueryWithStoredProcedureAsync<TeamMemberBO>("sp_GetTeamMembers", new Dictionary<string, object>
            {
                {"ProjectId", projectId},
                {"ApplicationUserId", applicationUserId},
                {"CurrentDate", currentDate},
                {"PageIndex", pageIndex},
                {"PageSize", pageSize},
                {"SearchText", searchText},
                {"OrderBy", orderby }
            },
            new Dictionary<string, Type>
            {
                {"Total", typeof(int)},
                {"TotalDisplay", typeof(int)}
            });

            return (result.result, int.Parse(result.outValues.ElementAt(0).Value.ToString()), int.Parse(result.outValues.ElementAt(1).Value.ToString()));
        }

        public string GetProjectOwnerEmail(Guid projectId)
        {
            var project=GetProjectDetailsById(projectId);
            return project.ProjectUsers.Select(x => x.ApplicationUser).FirstOrDefault().Email;
        }

        public IEnumerable<ApplicationUser> GetUsersByProjectIdAndRole(Guid projectId, ProjectRole role)
        {
            var users = _dbContext.ProjectUsers.Where(x => x.ProjectId == projectId && x.Role.Equals(role)).Select(x => x.ApplicationUser);
            return users;
        }

        public (IList<string> ownerProjectTitles, IList<string> workerProjectTitles) GetUserProjectTitles(Guid userId)
        {
            var ownerProjectTitles = GetProjectsByUserAndRole(userId, ProjectRole.Owner).Select(x => x.Title).ToList();
            var workerProjectTitles = GetProjectsByUserAndRole(userId, ProjectRole.Worker).Select(x => x.Title).ToList();
            
            return(ownerProjectTitles, workerProjectTitles);
        }
    }
}