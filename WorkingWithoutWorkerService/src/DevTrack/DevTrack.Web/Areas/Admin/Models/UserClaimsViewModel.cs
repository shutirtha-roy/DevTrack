using Autofac;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Services;
using DevTrack.Web.Models;

namespace DevTrack.Web.Areas.Admin.Models
{
    public class UserClaimsViewModel : BaseModel
    {
        public Guid UserId { get; set; }
        public IList<UserClaim> Claims { get; set; }

        private IClaimsService _claimService;

        public UserClaimsViewModel() : base()
        {

        }

        public UserClaimsViewModel(IClaimsService claimService)
        {
            _claimService = claimService;
        }

        public override void ResolveDependency(ILifetimeScope scope)
        {
            base.ResolveDependency(scope);
            _claimService = _scope.Resolve<IClaimsService>();
        }

        internal async Task SetData(Guid userId)
        {
            var userClaims = await _claimService.GetAllUserClaims(userId);
            UserId = userId;
            Claims = await _claimService.GetSelectedClaims(userClaims);
        }

        internal async Task UpdateExistingUserClaims(Guid userId)
        {
            await _claimService.RemoveClaimsFromUser(userId);
            await _claimService.AddClaimsToUser(userId, Claims);
        }

    }
}