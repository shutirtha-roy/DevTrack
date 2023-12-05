using DevTrack.Infrastructure.Entities;

namespace DevTrack.Infrastructure.Repositories
{
    public interface IActivityRepository : IRepository<Activity, Guid>
    {
        IEnumerable<Activity> GetActivitiesReport(Guid userId, Guid projectId, DateTime startDateTime, DateTime endDateTime);
        Task<long> GetprojectLoggedTime(Project project);
        Task<long> GetprojectLoggedTime(Project project, Guid userId);
        Task<long> GetmonthlyLoggedTime(int month, int currentYear, Guid userId);
        Task<long> GetmonthlyLoggedTime(int month, int currentYear, Guid userId, Guid projectId);
    }
}