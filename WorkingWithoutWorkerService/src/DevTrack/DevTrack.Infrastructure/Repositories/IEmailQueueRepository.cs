using DevTrack.Infrastructure.Entities;

namespace DevTrack.Infrastructure.Repositories
{
    public interface IEmailQueueRepository: IRepository<EmailQueue, Guid>
    {

    }
}