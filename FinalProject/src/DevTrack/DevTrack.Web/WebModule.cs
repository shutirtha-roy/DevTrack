using Autofac;
using DevTrack.Web.Areas.Admin.Models;
using DevTrack.Web.Areas.App.Models;
using DevTrack.Web.Models;

namespace DevTrack.Web
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoginModel>().AsSelf();

            builder.RegisterType<RegisterModel>().AsSelf();

            builder.RegisterType<SettingsEditModel>().AsSelf();

            builder.RegisterType<PasswordEditModel>().AsSelf();

            builder.RegisterType<ProjectCreateModel>().AsSelf();

            builder.RegisterType<ProjectEditModel>().AsSelf();

            builder.RegisterType<ProjectViewModel>().AsSelf();

            builder.RegisterType<ProjectListModel>().AsSelf();

            builder.RegisterType<InvitationModel>().AsSelf();

            builder.RegisterType<ProjectSearch>().AsSelf();

            builder.RegisterType<InvitationResponseModel>().AsSelf();

            builder.RegisterType<ProjectUserEmailListModel>().AsSelf();

            builder.RegisterType<ActivityRequestModel>().AsSelf();

            builder.RegisterType<ActivityProjectUserModel>().AsSelf();

            builder.RegisterType<UserModel>().AsSelf();

            builder.RegisterType<UserClaimsViewModel>().AsSelf();

            builder.RegisterType<UserProjectsModel>().AsSelf();

            base.Load(builder);
        }
    }
}