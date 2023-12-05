using DevTrack.Infrastructure.DbContexts;
using DevTrack.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Infrastructure.Repositories
{
    public class EmailQueueRepository: Repository<EmailQueue, Guid>, IEmailQueueRepository
    {
        private readonly IApplicationDbContext _dbContext;

        public EmailQueueRepository(IApplicationDbContext context) : base((DbContext)context)
        {
            _dbContext = context;
        }
    }
}