using DevTrack.Infrastructure.DbContexts;
using DevTrack.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Infrastructure.Repositories
{
    public class SettingsRepository : Repository<ApplicationUser, Guid>, ISettingsRepository
    {
        public SettingsRepository(IApplicationDbContext context) : base((DbContext)context)
        {

        }
    }
}
