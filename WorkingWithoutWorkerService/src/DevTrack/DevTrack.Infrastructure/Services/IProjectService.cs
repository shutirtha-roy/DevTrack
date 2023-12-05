using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Enum;

namespace DevTrack.Infrastructure.Services
{
    public interface IProjectService
    {
        Task CreateProject(Project project);
        Task EditProject(Project project, Guid userId);
        Task<Project> GetProject(Guid id, Guid userId);
        Task<Project> GetProjectDetails(Guid id);
        Task<IList<Project>> GetProjectsByUserIdAndRole(Guid userId, ProjectRole role);
        Task<IList<ApplicationUser>> GetUsersByProjectIdAndRole(Guid projectId, ProjectRole role);
        Task<(int total, int totalDisplay, IList<ProjectSummary> records)> GetProjects(int pageIndex,
            int pageSize, string searchText, Guid applicationUserId, string title, bool? isArchived, int? role, string orderby);
        Task<(int total, int totalDisplay, IList<TeamMember> records)> GetTeamMembers(int pageIndex, 
            int pageSize, string searchText, Guid projectId, Guid applicationUserId, string orderby);
        Task ArchiveProject(Guid id);
    }
}