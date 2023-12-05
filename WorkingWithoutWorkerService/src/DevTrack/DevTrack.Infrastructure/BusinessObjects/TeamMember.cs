using DevTrack.Infrastructure.Enum;

namespace DevTrack.Infrastructure.BusinessObjects
{
    public class TeamMember
    {
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public Guid ProjectId { get; set; }
        public double LoggedPastWeek { get; set; }
        public double LoggedThisMonth { get; set; }
        public double TotalLogged { get; set; }
        public InvitationStatus Status { get; set; }
    }
}