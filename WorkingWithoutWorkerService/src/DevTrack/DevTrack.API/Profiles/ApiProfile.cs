using AutoMapper;
using DevTrack.API.Models;
using ScreenCaptureBO = DevTrack.Infrastructure.BusinessObjects.ScreenCapture;
using UserInfoBO = DevTrack.Infrastructure.BusinessObjects.UserInfo;
using ProjectBO = DevTrack.Infrastructure.BusinessObjects.Project;
using ActivityBO = DevTrack.Infrastructure.BusinessObjects.Activity;
using WebcamCaptureBO = DevTrack.Infrastructure.BusinessObjects.WebcamCapture;
using RunningProgramBO = DevTrack.Infrastructure.BusinessObjects.RunningProgram;
using ActiveWindowsBO = DevTrack.Infrastructure.BusinessObjects.ActiveWindows;
using KeyboardActivitiesBO = DevTrack.Infrastructure.BusinessObjects.KeyboardActivities;
using MouseActivitiesBO = DevTrack.Infrastructure.BusinessObjects.MouseActivities;

namespace DevTrack.API.Profiles
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {
            CreateMap<UserInfoBO, UserInfo>();

            CreateMap<ProjectModel, ProjectBO>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.ProjectId))
                .ReverseMap();

            CreateMap<ActivityRequestModel, ActivityBO>()
                .ForMember(dest => dest.RunningPrograms, src => src.MapFrom(x => x.RunningProgram))
                .ReverseMap();

            CreateMap<ScreenCaptureModel, ScreenCaptureBO>()
                .ReverseMap();

            CreateMap<WebcamCaptureModel, WebcamCaptureBO>()
                .ReverseMap();

            CreateMap<RunningProgramModel, RunningProgramBO>()
                .ReverseMap();

            CreateMap<ActiveWindowsModel, ActiveWindowsBO>()
                .ReverseMap();

            CreateMap<KeyboardActivitiesModel, KeyboardActivitiesBO>()
                .ReverseMap();

            CreateMap<MouseActivitiesModel, MouseActivitiesBO>()
                .ReverseMap();
        }
    }
}