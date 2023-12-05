using Autofac;
using DevTrack.Infrastructure.DbContexts;
using DevTrack.Infrastructure.Repositories;
using DevTrack.Infrastructure.Services;
using DevTrack.Infrastructure.UnitOfWorks;

namespace DevTrack.Infrastructure
{
    public class InfrastructureModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public InfrastructureModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>().AsSelf()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationDbContext>().As<IApplicationDbContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<SettingsRepository>()
                .As<ISettingsRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AccountService>()
                .As<IAccountService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TokenService>()
                .As<ITokenService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationUnitOfWork>()
                .As<IApplicationUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationUnitOfWork>()
                .As<IApplicationUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<SettingsService>()
                .As<ISettingsService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<LocalPathService>()
                .As<IPathService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationUserManager>().AsSelf();

            builder.RegisterType<ApplicationSignInManager>().AsSelf();

            builder.RegisterType<ApplicationRoleManager>().AsSelf();

            builder.RegisterType<ImageService>()
                .As<IImageService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PasswordService>()
                .As<IPasswordService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ProjectRepository>()
                .As<IProjectRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ProjectService>()
                .As<IProjectService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TimeService>()
                .As<ITimeService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ActivityService>()
                .As<IActivityService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ActivityRepository>()
                .As<IActivityRepository>();

            builder.RegisterType<HtmlEmailService>()
                .As<IEmailService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EmailQueueService>()
                .As<IEmailQueueService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EmailQueueRepository>()
                .As<IEmailQueueRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<InvitationService>()
                .As<IInvitationService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<InvitationRepository>()
                .As<IInvitationRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ReportService>()
                .As<IReportService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ClaimsService>()
                .As<IClaimsService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserService>()
                .As<IUserService>()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}