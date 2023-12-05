using Autofac;
using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Enum;
using DevTrack.Infrastructure.Services;
using DevTrack.Web.Models;

namespace DevTrack.Web.Areas.App.Models
{
    public class InvitationModel:BaseModel
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public string? Email { get; set; }
        public InvitationStatus Status { get; set; }
        public DateTime Date { get; set; }

        private IMapper _mapper;
        private IInvitationService _invitationService;
        private ITimeService _timeService;

        public InvitationModel() : base()
        {

        }

        public InvitationModel(IInvitationService invitationService, IMapper mapper,ITimeService timeService, string email, Guid projectId)
        {
            _mapper = mapper;
            _invitationService = invitationService;
            _timeService=timeService;
            Email = email;
            ProjectId = projectId;
        }

        private void SetInvivationDateTime()
        {
            Date = _timeService.Now;
        }

        public override void ResolveDependency(ILifetimeScope scope)
        {
            base.ResolveDependency(scope);
            _invitationService = _scope.Resolve<IInvitationService>();
            _mapper = _scope.Resolve<IMapper>();
        }

        public async Task SaveToInvitationAsync()
        {
            SetInvivationDateTime();
            var model = _mapper.Map<Invitation>(this);
            model.Status = InvitationStatus.Pending;
            await _invitationService.SaveToInvitationAsync(model);
        }
    }
}