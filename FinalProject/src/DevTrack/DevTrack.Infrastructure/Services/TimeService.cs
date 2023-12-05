using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevTrack.Infrastructure.Services
{
    public class TimeService : ITimeService
    {
        public DateTime Now
        {
            get => DateTime.UtcNow;
        }

        public DateTime LocalTime
        {
            get => Now.ToLocalTime();
        }

        private const string DefaultCountryTimeZone = "(UTC+06:00) Bangladesh Standard Time";
        private const string DefaultUtc = "(UTC)";

        private async Task<string> GetTimeDifferenceBetweenDesiredTimeZoneAndUtc()
        {
            var timeChange = LocalTime - Now;

            return timeChange.ToString();
        }

        public async Task<string> GetLocalTimeZoneRegion()
        {
            var timeDifference = await GetTimeDifferenceBetweenDesiredTimeZoneAndUtc();
            var timeZone = await GenerateTimeZone(timeDifference);
            var requiredTimeZone = await GetRequiredTimeZone(timeZone);

            return requiredTimeZone;
        }

        public async Task<DateTime> GetTimeFromTimeZone(string timeZoneRegion)
        {
            try
            {
                var timezonne = timeZoneRegion.Remove(0, timeZoneRegion.IndexOf(")") + 2);
                var timeInfo = TimeZoneInfo.FindSystemTimeZoneById(timezonne);
                var timeZoneDate = TimeZoneInfo.ConvertTimeFromUtc(Now, timeInfo);

                return timeZoneDate;
            }
            catch (Exception)
            {
                throw new TimeZoneNotFoundException("Your required time zone is not found");
            }
        }

        public async Task<string> GetRequiredTimeZone(string timeChangeBetweenSelectedAndUtc)
        {
            var desiredTimeZone = "";

            foreach (TimeZoneInfo zone in TimeZoneInfo.GetSystemTimeZones())
            {
                var zoneLen = zone.DisplayName.IndexOf(")");
                var timeZone = zone.DisplayName.Substring(0, zoneLen + 1);

                if (timeZone == "(UTC)")
                    continue;

                if (timeZone.Contains(timeChangeBetweenSelectedAndUtc))
                {
                    desiredTimeZone = $"{timeZone} {zone.Id}";
                    break;
                }  
            }

            var requiredTimezone = desiredTimeZone.Equals("") ? DefaultCountryTimeZone : desiredTimeZone;

            return requiredTimezone;
        }

        public async Task<string> GenerateTimeZone(string timeChangeBetweenLocalAndUtc)
        {
            try
            {
                var splitTime = timeChangeBetweenLocalAndUtc.Split(":");
                var timeZone = $"{splitTime[0]}:{splitTime[1]}";
                return timeZone;
            }
            catch
            {
                return DefaultCountryTimeZone;
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetAllTimeZone()
        {
            var zones = new List<SelectListItem>();

            foreach (TimeZoneInfo zone in TimeZoneInfo.GetSystemTimeZones())
            {
                var zoneLen = zone.DisplayName.IndexOf(")");
                var timeZone = zone.DisplayName.Substring(0, zoneLen + 1);

                if (timeZone == DefaultUtc)
                    continue;
                
                zones.Add(new SelectListItem { Value = $"{timeZone} {zone.Id}", Text = $"{timeZone} {zone.Id}" });
            }

            return zones;
        }

        public async Task<DateOnly[]> GetAllSelectedDates(DateTime startDateTime, DateTime endDateTime)
        {
            var selectedDates = new List<DateOnly>();
            var startDate = DateOnly.FromDateTime(startDateTime);
            var endDate = DateOnly.FromDateTime(endDateTime);

            while(startDate < endDate)
            {
                selectedDates.Add(startDate);
                startDate = startDate.AddDays(1);
            }

            return selectedDates.ToArray();
        }
    }
}