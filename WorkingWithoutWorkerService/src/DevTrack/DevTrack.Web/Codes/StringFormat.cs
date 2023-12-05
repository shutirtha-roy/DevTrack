namespace DevTrack.Web.Codes
{
    public static class StringFormat
    {
        public static string ToHourMinute(this double totalHour)
        {
            TimeSpan t = TimeSpan.FromHours(totalHour);
            return string.Format("{0:00}:{1:00}", t.Hours, t.Minutes);
        }
    }
}