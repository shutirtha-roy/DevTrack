namespace DevTrack.Web.Areas.App.Models
{
    public class KeyboardActivitiesModel
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public int TotalHits { get; set; }
        public string? KeyCounts { get; set; }
    }
}