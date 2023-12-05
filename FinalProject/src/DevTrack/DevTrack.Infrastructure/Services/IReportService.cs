namespace DevTrack.Infrastructure.Services
{
    public interface IReportService
    {
        Task<Dictionary<(TimeOnly startTime, TimeOnly endTime), (string hourSlot, IList<T> reportSlot)?>> GetEmptyReportSlotOfOneDay<T>();
        Task<Dictionary<DateOnly, Dictionary<(TimeOnly startTime, TimeOnly endTime), (string hourSlot, IList<T> reportSlot)?>>> GetReportValuesOfDates<T>(DateOnly date);
    }
}