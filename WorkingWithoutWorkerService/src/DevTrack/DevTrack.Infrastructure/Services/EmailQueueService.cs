using AutoMapper;
using DevTrack.Infrastructure.UnitOfWorks;
using Microsoft.Extensions.Configuration;
using EmailQueueBO = DevTrack.Infrastructure.BusinessObjects.EmailQueue;
using EmailQueueEO = DevTrack.Infrastructure.Entities.EmailQueue;

namespace DevTrack.Infrastructure.Services
{
    public class EmailQueueService : IEmailQueueService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        private readonly string _emailTemplate;
        private readonly string _emailSubject;

        public EmailQueueService(IMapper mapper, IApplicationUnitOfWork applicationUnitOfWork, IConfiguration configuration)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
            _emailTemplate = configuration.GetValue<string>("InvitationEmailTemplate");
            _emailSubject = configuration.GetValue<string>("InvitationEmailSubject");
        }

        public async Task SaveToQueueAsync(EmailQueueBO model)
        {
            var emailQueueEO = _mapper.Map<EmailQueueEO>(model);
            emailQueueEO.Template = _emailTemplate;
            emailQueueEO.Subject = _emailSubject;
            _applicationUnitOfWork.EmailQueue.Add(emailQueueEO);
            _applicationUnitOfWork.Save();
        }
    }
}