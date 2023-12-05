using DevTrack.Infrastructure.BusinessObjects;

namespace DevTrack.Infrastructure.Services
{
    public interface IInvitationService
    {
        Task SaveToInvitationAsync(Invitation model);
        Task SaveInvitationUserToProjectUser(string email, Guid projectId);
        Task<Invitation> GetUserInvitation(Guid userId, Guid projectId);
        Task<Invitation> LoadInvitationData(Guid invitationId);
        Task AcceptInvitation(Invitation invitation);
        Task RejectInvitation(Invitation invitation);
        Task<List<string>> GetUserEmails(UserEmailList model);
    }
}