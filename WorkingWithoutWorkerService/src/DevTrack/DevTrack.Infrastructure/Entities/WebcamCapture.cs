﻿namespace DevTrack.Infrastructure.Entities
{
    public class WebcamCapture : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public Activity? Activity { get; set; }
        public string? Image { get; set; }
    }
}