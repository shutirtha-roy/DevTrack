using DevTrack.Infrastructure.BusinessObjects;

namespace DevTrack.Infrastructure.Services
{
    public interface IActivityService
    {
        Task CreateActivity(Activity activity);
        Task<Dictionary<DateOnly, Dictionary<(TimeOnly startTime, TimeOnly endTime), (string hourSlot, IList<Activity> reportSlot)?>>> 
            GetActivitiesReport(Guid userId, Guid projectId, DateTime startTime, DateTime endTime);
    }
}