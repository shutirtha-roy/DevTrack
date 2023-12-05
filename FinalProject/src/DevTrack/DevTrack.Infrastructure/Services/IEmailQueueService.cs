using DevTrack.Infrastructure.BusinessObjects;

namespace DevTrack.Infrastructure.Services
{
    public interface IEmailQueueService
    {
        Task SaveToQueueAsync(EmailQueue model);
        Task SendProjectInvitationEmail();
    }
}