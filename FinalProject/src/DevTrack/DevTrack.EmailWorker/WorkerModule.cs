using Autofac;
using DevTrack.EmailWorker.Models;

namespace DevTrack.EmailWorker
{
    public class WorkerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmailSendModel>().AsSelf();

            base.Load(builder);
        }
    }
}