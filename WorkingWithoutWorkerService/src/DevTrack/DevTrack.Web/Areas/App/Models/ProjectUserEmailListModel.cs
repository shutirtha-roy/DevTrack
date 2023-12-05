using Autofac;
using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Services;
using DevTrack.Web.Models;

namespace DevTrack.Web.Areas.App.Models
{
    public class ProjectUserEmailListModel : BaseModel
    {
        public Guid ProjectId { get; set; }
        public List<string> Emails { get; set; }

        private IMapper _mapper;
        private IInvitationService _invitationService;

        public ProjectUserEmailListModel() : base()
        {

        }

        public ProjectUserEmailListModel(IInvitationService invitationService, IMapper mapper, Guid projectId)
        {
            _mapper = mapper;
            _invitationService = invitationService;
            ProjectId = projectId;
        }


        public override void ResolveDependency(ILifetimeScope scope)
        {
            base.ResolveDependency(scope);
            _invitationService = _scope.Resolve<IInvitationService>();
            _mapper = _scope.Resolve<IMapper>();
        }

        public async Task<List<string>> GetUserEmails()
        {
            var model = _mapper.Map<UserEmailList>(this);
            Emails=await _invitationService.GetUserEmails(model);
            return Emails;
        }
    }
}