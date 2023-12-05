using DevTrack.Infrastructure.Entities;

namespace DevTrack.Infrastructure.Repositories
{
    public interface IInvitationRepository: IRepository<Invitation, Guid>
    {
        Invitation GetInvitationDetailsById(Guid id);
        Invitation GetPendingInvitationDetailsByEmail(string userEmail, Guid projectId);
        int GetPendingInvitationCountByEmail(string userEmail, Guid projectId);
        IList<string> GetPendingInvitationEmailsByProjectId(Guid projectId);
    }
}