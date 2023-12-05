namespace DevTrack.Infrastructure.BusinessObjects
{
    public class Report
    {
        public Guid? UniqueId { get; set; }
        public DateOnly ReportDate { get; set; }
        public IList<string> ScreenCaptureList { get; set; }
        public IList<string> WebcamCaptureList { get; set; }
        public IList<ActiveWindows> ActiveWindowsList { get; set; }
        public IList<RunningProgram> RunningProgramList { get; set; }
        public IList<string> KeyboardKeysList { get; set; }
        public int TotalMouseActivity { get; set; }
        public int TotalRunningPrograms { get; set; }
        public int TotalActiveWindows { get; set; }
        public string ActiveWindowsValues { get; set; }
        public string ActiveRunningValues { get; set; }
    }
}
