using Autofac;
using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Enum;
using DevTrack.Infrastructure.Services;
using DevTrack.Web.Models;
using System.Data;

namespace DevTrack.Web.Areas.App.Models
{
    public class InvitationResponseModel:BaseModel
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public string? Email { get; set; }
        public InvitationStatus Status { get; set; }
        public DateTime Date { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectAdminName { get; set; }
        public DateTime ProjectCreateDate { get; set; }
        public ProjectRole Role { get; set; }

        private IMapper _mapper;
        private IInvitationService _invitationService;

        public InvitationResponseModel() : base()
        {

        }

        public InvitationResponseModel(IInvitationService invitationService, IMapper mapper, ITimeService timeService)
        {
            _mapper = mapper;
            _invitationService = invitationService;
            _timeService = timeService;
        }

        public override void ResolveDependency(ILifetimeScope scope)
        {
            base.ResolveDependency(scope);
            _invitationService = _scope.Resolve<IInvitationService>();
            _mapper = _scope.Resolve<IMapper>();
        }

        public async Task LoadInvitationData(Guid id)
        {
            var invitationId = id;
            var invitation = await _invitationService.LoadInvitationData(invitationId);
            if (invitation != null)
            {
                _mapper.Map(invitation, this);
                ProjectTitle = Project.Title;
                ProjectAdminName = Project.ProjectUsers.Select(x => x.ApplicationUser.Name).FirstOrDefault();
                ProjectCreateDate = Project.CreatedDate;
                Role = Project.ProjectUsers.Select(x => x.Role).FirstOrDefault();
            }
        }

        public async Task AcceptInvitation()
        {
            var invitation = _mapper.Map<Invitation>(this);
            await _invitationService.AcceptInvitation(invitation);
        }

        public async Task RejectInvitation()
        {
            var invitation = _mapper.Map<Invitation>(this);
            await _invitationService.RejectInvitation(invitation);
        }
    }
}