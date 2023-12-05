using System.Globalization;

namespace DevTrack.Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private const int _totalHour = 24;
        private const int _totalColumn = 6;
        private const int _timeSlot = 10;
        private const int _hourInMinuite = 60;
        private TimeOnly _initialStartTime;
        private TimeOnly _initialEndTime;
        private string _timeSlotInterval;

        public async Task<Dictionary<(TimeOnly startTime, TimeOnly endTime), (string hourSlot, IList<T> reportSlot)?>> GetEmptyReportSlotOfOneDay<T>()
        {
            var reportSlots = new Dictionary<(TimeOnly startTime, TimeOnly endTime), (string hourSlot, IList<T> reportSlot)?>();
            _initialStartTime = new TimeOnly(0, 0);
            _initialEndTime = new TimeOnly(0, 10);

            for (int i = 1; i <= _totalHour; i++)
            {
                _timeSlotInterval = $"{_initialStartTime.ToString("h:mm tt", CultureInfo.InvariantCulture)}-{_initialStartTime.AddMinutes(_hourInMinuite - 1).ToString("h:mm tt", CultureInfo.InvariantCulture)}";

                for (int j = 1; j <= _totalColumn; j++)
                {
                    reportSlots[(_initialStartTime, _initialEndTime)] = (_timeSlotInterval, new List<T>());
                    _initialStartTime = _initialStartTime.AddMinutes(_timeSlot);
                    _initialEndTime = _initialEndTime.AddMinutes(_timeSlot);
                }
            }

            return reportSlots;
        }

        public async Task<Dictionary<DateOnly, Dictionary<(TimeOnly startTime, TimeOnly endTime), (string hourSlot, IList<T> reportSlot)?>>> GetReportValuesOfDates<T>(DateOnly date)
        {
            var allSelectedReportSlots = new Dictionary<DateOnly, Dictionary<(TimeOnly startTime, TimeOnly endTime), (string hourSlot, IList<T> reportSlot)?>>();
            var fullDayTimeSlot = await GetEmptyReportSlotOfOneDay<T>();

            allSelectedReportSlots.Add(date, fullDayTimeSlot);

            return allSelectedReportSlots;
        }
    }
}