using DevTrack.Infrastructure.Repositories;

namespace DevTrack.Infrastructure.UnitOfWorks
{
    public interface IApplicationUnitOfWork : IUnitOfWork
    {
        ISettingsRepository Settings { get; }
        IProjectRepository Projects { get; }
        IActivityRepository Activities { get; }
        IEmailQueueRepository EmailQueue { get; }
        IInvitationRepository Invitations { get; }
    }
}