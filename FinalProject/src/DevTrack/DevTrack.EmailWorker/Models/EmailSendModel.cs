using Autofac;
using DevTrack.Infrastructure.Services;

namespace DevTrack.EmailWorker.Models
{
    public class EmailSendModel
    {
        private ILifetimeScope _scope;
        private IEmailQueueService _emailQueueService;


        public EmailSendModel(IEmailQueueService emailQueueService)
        {
            _emailQueueService = emailQueueService;
        }

        internal void ResolveDependency(ILifetimeScope scope)
        {
            _scope = scope;
            _emailQueueService = _scope.Resolve<IEmailQueueService>();
        }

        public async Task Send()
        {
            await _emailQueueService.SendProjectInvitationEmail();
        }
    }
}