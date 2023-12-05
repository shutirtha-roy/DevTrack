using DevTrack.Infrastructure.Enum;

namespace DevTrack.Infrastructure.BusinessObjects
{
    public class ProjectSummary
    {
        public Guid ApplicationUserId { get; set; }
        public ProjectRole Role { get; set; }
        public InvitationStatus Status { get; set; }
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public int TotalMembers { get; set; }
        public double TotalLogHours { get; set; }
    }
}