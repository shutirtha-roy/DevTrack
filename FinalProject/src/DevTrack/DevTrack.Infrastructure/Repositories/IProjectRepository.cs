using DevTrack.Infrastructure.Entities;
using DevTrack.Infrastructure.Enum;
using ProjectSummaryBO = DevTrack.Infrastructure.BusinessObjects.ProjectSummary;
using TeamMemberBO = DevTrack.Infrastructure.BusinessObjects.TeamMember;

namespace DevTrack.Infrastructure.Repositories
{
    public interface IProjectRepository : IRepository<Project, Guid>
    {
        Project GetProjectDetailsById(Guid id);
        string GetProjectOwnerEmail(Guid projectId);
        IEnumerable<Project> GetProjectsByUserAndRole(Guid userId, ProjectRole role);
        IList<string> GetUserEmailsByProjectID(Guid projectId);
        Task<(IList<ProjectSummaryBO> data, int total, int totalDisplay)> GetProjects(int pageIndex,
            int pageSize, string searchText, Guid applicationUserId, string title, bool? isArchived, int? role, string orderby);
        Task<(IList<TeamMemberBO> data, int total, int totalDisplay)> GetTeamMembers(int pageIndex,
            int pageSize, string searchText, Guid projectId, Guid applicationUserId, DateTime currentDate, string orderby);
        IEnumerable<ApplicationUser> GetUsersByProjectIdAndRole(Guid projectId, ProjectRole role);
        (IList<string> ownerProjectTitles, IList<string> workerProjectTitles) GetUserProjectTitles(Guid userId);
    }
}