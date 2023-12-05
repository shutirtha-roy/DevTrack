﻿namespace DevTrack.Infrastructure.Entities
{
    public class ActiveWindows : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public Activity? Activity { get; set; }
        public string? ProcessName { get; set; }
        public string? MainWindowTitle { get; set; }
    }
}