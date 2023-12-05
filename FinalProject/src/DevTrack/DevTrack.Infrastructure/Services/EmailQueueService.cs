using AutoMapper;
using DevTrack.Infrastructure.Enum;
using DevTrack.Infrastructure.Templates.Emails.ProjectInvitation;
using DevTrack.Infrastructure.UnitOfWorks;
using Microsoft.Extensions.Configuration;
using EmailQueueBO = DevTrack.Infrastructure.BusinessObjects.EmailQueue;
using EmailQueueEO = DevTrack.Infrastructure.Entities.EmailQueue;

namespace DevTrack.Infrastructure.Services
{
    public class EmailQueueService : IEmailQueueService
    {
        private readonly IConfiguration _configuration;
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly string _emailTemplate;
        private readonly string _emailSubject;
        private readonly string _applicationBaseUrl;

        public EmailQueueService(IMapper mapper, IApplicationUnitOfWork applicationUnitOfWork, IConfiguration configuration,
            IEmailService emailService)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _emailService = emailService;
            _applicationBaseUrl = _configuration.GetValue<string>("ApplicationBaseUrl");
            _emailSubject = _configuration.GetValue<string>("InvitationEmailSubject");
            _emailTemplate = _configuration.GetValue<string>("InvitationEmailTemplate");
        }

        public async Task SaveToQueueAsync(EmailQueueBO model)
        {
            var emailQueueEO = _mapper.Map<EmailQueueEO>(model);
            emailQueueEO.Template = _emailTemplate;
            emailQueueEO.Subject = _emailSubject;
            _applicationUnitOfWork.EmailQueue.Add(emailQueueEO);
            _applicationUnitOfWork.Save();
        }

        public async Task SendProjectInvitationEmail()
        {
            var entities = _applicationUnitOfWork.EmailQueue.Get(x => x.SendStatus == EmailSendStatus.NotSent, "");
            foreach (var entity in entities)
            {
                var body = new ProjectInvitation(_applicationBaseUrl).TransformText();

                var isSent = await _emailService.SendSingleEmail(entity.Email, entity.Email, entity.Subject, body);

                if (isSent)
                    entity.SendStatus = EmailSendStatus.Sent;

                _applicationUnitOfWork.Save();
            }
        }
    }
}