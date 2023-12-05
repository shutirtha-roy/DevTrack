namespace DevTrack.Web.Areas.App.Models
{
    public class RunningProgramModel
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public string? MainWindowTitle { get; set; }
        public string? ProcessName { get; set; }     
    }
}