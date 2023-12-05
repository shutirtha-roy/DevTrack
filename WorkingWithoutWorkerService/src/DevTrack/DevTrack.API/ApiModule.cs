using Autofac;
using DevTrack.API.Models;

namespace DevTrack.API
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoginModel>().AsSelf();

            builder.RegisterType<ProjectListModel>().AsSelf();

            builder.RegisterType<ActivityRequestModel>().AsSelf();

            base.Load(builder);
        }
    }
}