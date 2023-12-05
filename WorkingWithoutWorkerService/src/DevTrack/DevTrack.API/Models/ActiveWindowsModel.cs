namespace DevTrack.API.Models
{
    public class ActiveWindowsModel
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }     
        public string? ProcessName { get; set; }
        public string? MainWindowTitle { get; set; }
    }
}