namespace DevTrack.Infrastructure.Entities
{
    public class MouseActivities : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public Activity? Activity { get; set; }
        public int TotalHits { get; set; }
    }
}