using AutoMapper;
using DevTrack.Infrastructure.Entities;
using DevTrack.Infrastructure.Enum;
using DevTrack.Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Identity;
using InvitationBO = DevTrack.Infrastructure.BusinessObjects.Invitation;
using InvitationEO = DevTrack.Infrastructure.Entities.Invitation;
using EmailQueueBO = DevTrack.Infrastructure.BusinessObjects.EmailQueue;
using UserEmailListBO = DevTrack.Infrastructure.BusinessObjects.UserEmailList;

namespace DevTrack.Infrastructure.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        private UserManager<ApplicationUser> _userManager;
        private IEmailQueueService _emailQueueService;

        public InvitationService(IMapper mapper, IApplicationUnitOfWork applicationUnitOfWork, UserManager<ApplicationUser> userManager, IEmailQueueService emailQueueService)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _emailQueueService = emailQueueService;
        }

        public async Task SaveInvitationUserToProjectUser(string email, Guid projectId)
        {
            var user = _userManager.Users.Where(x => x.Email == email).Select(s => s).FirstOrDefault();
            var userId = user.Id;
            var project = _applicationUnitOfWork.Projects.GetProjectDetailsById(projectId);
            var projectUser = new ProjectUser
            {
                Project = project,
                ProjectId = projectId,
                ApplicationUser = user,
                ApplicationUserId = userId,
                Role = ProjectRole.Worker
            };
            project.ProjectUsers.Add(projectUser);
            _applicationUnitOfWork.Save();
        }

        public async Task SaveToInvitationAsync(InvitationBO model)
        {
            var invitationEO = _mapper.Map<InvitationEO>(model);
            var project = _applicationUnitOfWork.Projects.GetProjectDetailsById(invitationEO.ProjectId);
            var ownerEmail = _applicationUnitOfWork.Projects.GetProjectOwnerEmail(invitationEO.ProjectId);
            if (invitationEO.Email != ownerEmail)
            {
                var count = _applicationUnitOfWork.Invitations.GetPendingInvitationCountByEmail(invitationEO.Email, invitationEO.ProjectId);
                if (count == 0)
                {
                    var emailQueueBO = _mapper.Map<EmailQueueBO>(model);
                    await _emailQueueService.SaveToQueueAsync(emailQueueBO);

                    invitationEO.Project = project;
                    _applicationUnitOfWork.Invitations.Add(invitationEO);
                    _applicationUnitOfWork.Save();
                    await SaveInvitationUserToProjectUser(invitationEO.Email, invitationEO.ProjectId);
                }
            }
        }

        public async Task RemoveInvitationUserFromProjectUser(string email, Guid projectId)
        {
            var userId = _userManager.Users.Where(x => x.Email == email).Select(s => s).FirstOrDefault().Id;
            var project = _applicationUnitOfWork.Projects.GetProjectDetailsById(projectId);
            var projectUser = project.ProjectUsers.Where(x => x.ApplicationUserId == userId && x.ProjectId == projectId).Select(s => s).FirstOrDefault();
            project.ProjectUsers.Remove(projectUser);
            _applicationUnitOfWork.Save();
        }

        public async Task<InvitationBO> GetUserInvitation(Guid userId, Guid projectId)
        {
            var userEmail = _userManager.Users.Where(x => x.Id == userId).Select(s => s.Email).FirstOrDefault();
            var invitationEO = _applicationUnitOfWork.Invitations.GetPendingInvitationDetailsByEmail(userEmail, projectId);
            var invitation = _mapper.Map<InvitationBO>(invitationEO);

            return invitation;
        }

        public async Task<InvitationBO> LoadInvitationData(Guid invitationId)
        {
            var invitationEO = _applicationUnitOfWork.Invitations.GetInvitationDetailsById(invitationId);
            var invitation = _mapper.Map<InvitationBO>(invitationEO);

            return invitation;
        }

        public async Task AcceptInvitation(InvitationBO invitation)
        {
            var invitationEO = _mapper.Map<InvitationEO>(invitation);
            invitationEO.Project = _applicationUnitOfWork.Projects.GetProjectDetailsById(invitationEO.ProjectId);
            _applicationUnitOfWork.Invitations.Edit(invitationEO);
            _applicationUnitOfWork.Save();
        }

        public async Task RejectInvitation(InvitationBO invitation)
        {
            var invitationEO = _mapper.Map<InvitationEO>(invitation);
            invitationEO.Project = _applicationUnitOfWork.Projects.GetProjectDetailsById(invitationEO.ProjectId);
            _applicationUnitOfWork.Invitations.Edit(invitationEO);
            _applicationUnitOfWork.Save();
            await RemoveInvitationUserFromProjectUser(invitationEO.Email, invitationEO.ProjectId);
        }

        public async Task<List<string>> GetUserEmails(UserEmailListBO model)
        {
            var projectUsers = _applicationUnitOfWork.Projects.GetUserEmailsByProjectID(model.ProjectId);
            return projectUsers;
        }
    }
}