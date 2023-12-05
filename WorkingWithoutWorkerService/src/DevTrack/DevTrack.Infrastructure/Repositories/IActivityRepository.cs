using DevTrack.Infrastructure.Entities;

namespace DevTrack.Infrastructure.Repositories
{
    public interface IActivityRepository : IRepository<Activity, Guid>
    {
        IEnumerable<Activity> GetActivitiesReport(Guid userId, Guid projectId, DateTime startDateTime, DateTime endDateTime);
    }
}