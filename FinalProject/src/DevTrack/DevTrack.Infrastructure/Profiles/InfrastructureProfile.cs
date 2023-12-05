using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using ApplicationUserBO = DevTrack.Infrastructure.BusinessObjects.ApplicationUser;
using ApplicationUserEO = DevTrack.Infrastructure.Entities.ApplicationUser;
using ProjectEO = DevTrack.Infrastructure.Entities.Project;
using ProjectUserEO = DevTrack.Infrastructure.Entities.ProjectUser;
using ActivityEO = DevTrack.Infrastructure.Entities.Activity;
using ActivityBO = DevTrack.Infrastructure.BusinessObjects.Activity;
using ScreenCaptureBO = DevTrack.Infrastructure.BusinessObjects.ScreenCapture;
using WebcamCaptureBO = DevTrack.Infrastructure.BusinessObjects.WebcamCapture;
using RunningProgramBO = DevTrack.Infrastructure.BusinessObjects.RunningProgram;
using ActiveWindowsBO = DevTrack.Infrastructure.BusinessObjects.ActiveWindows;
using KeyboardActivitiesBO = DevTrack.Infrastructure.BusinessObjects.KeyboardActivities;
using MouseActivitiesBO = DevTrack.Infrastructure.BusinessObjects.MouseActivities;
using ScreenCaptureEO = DevTrack.Infrastructure.Entities.ScreenCapture;
using WebcamCaptureEO = DevTrack.Infrastructure.Entities.WebcamCapture;
using RunningProgramEO = DevTrack.Infrastructure.Entities.RunningProgram;
using ActiveWindowsEO = DevTrack.Infrastructure.Entities.ActiveWindows;
using KeyboardActivitiesEO = DevTrack.Infrastructure.Entities.KeyboardActivities;
using MouseActivitiesEO = DevTrack.Infrastructure.Entities.MouseActivities;
using EmailQueueEO = DevTrack.Infrastructure.Entities.EmailQueue;
using EmailQueueBO = DevTrack.Infrastructure.BusinessObjects.EmailQueue;
using InvitationEO = DevTrack.Infrastructure.Entities.Invitation;
using InvitationBO = DevTrack.Infrastructure.BusinessObjects.Invitation;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace DevTrack.Infrastructure.Profiles
{
    public class InfrastructureProfile : Profile
    {
        public InfrastructureProfile()
        {
            CreateMap<ApplicationUserBO, ApplicationUserEO>()
                .ForMember(x => x.Image, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ApplicationUserEO, UserInfo>()
                .ForMember(dest => dest.UserId, src => src.MapFrom(x => x.Id));

            CreateMap<ProjectEO, Project>()
                .ReverseMap();

            CreateMap<ProjectUserEO, ProjectUser>()
               .ReverseMap();

            CreateMap<ActivityEO, ActivityBO>()
               .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.ApplicationUserId))
               .ForMember(dest => dest.ActivityId, opt => opt.MapFrom(src => src.Id))
               .ReverseMap();

            CreateMap<ScreenCaptureEO, ScreenCaptureBO>()
               .ReverseMap();

            CreateMap<WebcamCaptureEO, WebcamCaptureBO>()
                .ReverseMap();

            CreateMap<RunningProgramEO, RunningProgramBO>()
                .ReverseMap();

            CreateMap<ActiveWindowsEO, ActiveWindowsBO>()
                .ReverseMap();

            CreateMap<KeyboardActivitiesEO, KeyboardActivitiesBO>()
                .ReverseMap();

            CreateMap<MouseActivitiesEO, MouseActivitiesBO>()
                .ReverseMap();

            CreateMap<EmailQueueEO, EmailQueueBO>()
                .ReverseMap();

            CreateMap<InvitationEO, InvitationBO>()
                .ReverseMap();

            CreateMap<InvitationBO, EmailQueueBO>()
                .ForMember(dest => dest.Id, act => act.Ignore())
                .ReverseMap();
        }
    }
}