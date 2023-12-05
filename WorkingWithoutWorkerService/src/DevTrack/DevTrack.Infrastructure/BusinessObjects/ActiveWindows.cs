namespace DevTrack.Infrastructure.BusinessObjects
{
    public class ActiveWindows
    {
        public string? ProcessName { get; set; }
        public string? MainWindowTitle { get; set; }
        public DateTime Time { get; set; }
    }
}