namespace DevTrack.Infrastructure.Entities
{
    public class KeyboardActivities : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public Activity? Activity { get; set; }
        public int TotalHits { get; set; }
        public string? KeyCounts { get; set; }
    }
}