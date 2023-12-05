using Autofac;
using DevTrack.EmailWorker.Models;

namespace DevTrack.EmailWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ILifetimeScope _scope;

        public Worker(ILogger<Worker> logger, ILifetimeScope scope)
        {
            _logger = logger;
            _scope = scope;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var model = _scope.Resolve<EmailSendModel>();
                model.ResolveDependency(_scope);
                await model.Send();
                await Task.Delay(1000 * 60, stoppingToken);
            }
        }
    }
}