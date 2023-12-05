using DevTrack.Infrastructure.DbContexts;
using DevTrack.Infrastructure.Entities;
using DevTrack.Infrastructure.Enum;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Infrastructure.Repositories
{
    public class InvitationRepository: Repository<Invitation, Guid>, IInvitationRepository
    {
        private readonly IApplicationDbContext _dbContext;

        public InvitationRepository(IApplicationDbContext context) : base((DbContext)context)
        {
            _dbContext = context;
        }

        public Invitation GetInvitationDetailsById(Guid id)
        {
            return _dbContext.Invitations.Include(x => x.Project).ThenInclude(x => x.ProjectUsers).ThenInclude(x=>x.ApplicationUser).Where(y => y.Id == id).FirstOrDefault();
        }

        public Invitation GetPendingInvitationDetailsByEmail(string userEmail, Guid projectId)
        {
            return _dbContext.Invitations.Include(x => x.Project).ThenInclude(x => x.ProjectUsers).ThenInclude(x => x.ApplicationUser).Where(y => y.Email == userEmail && y.ProjectId == projectId && (y.Status == InvitationStatus.Pending || y.Status == 0)).FirstOrDefault();
        }

        public int GetPendingInvitationCountByEmail(string userEmail, Guid projectId)
        {
            return _dbContext.Invitations.Count(x => x.ProjectId == projectId && x.Email == userEmail && x.Status != InvitationStatus.Rejected && x.Status != InvitationStatus.Expired);
        }

        public IList<string> GetPendingInvitationEmailsByProjectId(Guid projectId)
        {
            return _dbContext.Invitations.Where(x => x.ProjectId == projectId && x.Status == InvitationStatus.Pending).Select(s=>s.Email).ToList();
        }
    }
}