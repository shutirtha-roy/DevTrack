using DevTrack.Infrastructure.Enum;

namespace DevTrack.Infrastructure.BusinessObjects
{
    public class ProjectUser
    {
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }
        public ProjectRole Role { get; set; }
    }
}