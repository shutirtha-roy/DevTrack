using DevTrack.Infrastructure.DbContexts;
using DevTrack.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Infrastructure.UnitOfWorks
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public ISettingsRepository Settings { get; private set; }
        public IProjectRepository Projects { get; private set; }
        public IActivityRepository Activities { get; private set; }
        public IEmailQueueRepository EmailQueue { get; private set; }
        public IInvitationRepository Invitations { get; private set; }

        public ApplicationUnitOfWork(IApplicationDbContext dbContext, ISettingsRepository settingsRepository, IProjectRepository projectRepository,
            IActivityRepository activitiesRepository, IEmailQueueRepository emailQueueRepository, IInvitationRepository invitationRepository)
            : base((DbContext)dbContext)
        {
            Settings = settingsRepository;
            Projects = projectRepository;
            Activities = activitiesRepository;
            EmailQueue = emailQueueRepository;
            Invitations = invitationRepository;
        }
    }
}