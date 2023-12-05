using DevTrack.Infrastructure.Enum;

namespace DevTrack.Web.Areas.App.Models
{
    public class ProjectSearch
    {
        public Guid ApplicationUserId { get; set; }
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public bool? IsArchived { get; set; }
        public int? Role { get; set; }
    }
}