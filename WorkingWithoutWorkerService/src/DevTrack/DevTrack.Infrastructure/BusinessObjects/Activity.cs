namespace DevTrack.Infrastructure.BusinessObjects
{
    public class Activity 
    {
        public Guid ActivityId { get; set; }
        public Guid UserId { get; set; }     
        public Guid ProjectId { get; set; }
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsOnline { get; set; }
        public ScreenCapture? ScreenCapture { get; set; }
        public WebcamCapture? WebcamCapture { get; set; }
        public IList<RunningProgram>? RunningPrograms { get; set; }
        public IList<ActiveWindows>? ActiveWindows { get; set; }
        public KeyboardActivities? KeyboardActivity { get; set; }
        public MouseActivities? MouseActivity { get; set; }

        public DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            var modTicks = dt.Ticks % d.Ticks;
            var delta = modTicks != 0 ? d.Ticks - modTicks : 0;
            return new DateTime(dt.Ticks + delta, dt.Kind);
        }

        public DateTime RoundDown(DateTime dt, TimeSpan d)
        {
            var delta = dt.Ticks % d.Ticks;
            return new DateTime(dt.Ticks - delta, dt.Kind);
        }

        public async Task RoundOffMinutes()
        {
            StartTime = RoundDown(StartTime, TimeSpan.FromMinutes(10));
            EndTime = RoundUp(EndTime, TimeSpan.FromMinutes(10));

            if (StartTime.AddMinutes(10) != EndTime)
            {
                StartTime = EndTime.AddMinutes(-10);
            }
        }
    }
}