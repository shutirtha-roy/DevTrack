using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevTrack.Infrastructure.Services
{
    public interface ITimeService
    {
        DateTime Now { get; }
        DateTime LocalTime { get; }
        Task<string> GetLocalTimeZoneRegion();
        Task<DateTime> GetTimeFromTimeZone(string timeZoneRegion);
        Task<string> GetRequiredTimeZone(string timeChangeBetweenSelectedAndUtc);
        Task<IEnumerable<SelectListItem>> GetAllTimeZone();
        Task<DateOnly[]> GetAllSelectedDates(DateTime startDateTime, DateTime endDateTime);
    }
}