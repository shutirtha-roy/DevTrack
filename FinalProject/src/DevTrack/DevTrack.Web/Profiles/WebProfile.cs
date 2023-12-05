using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Web.Areas.App.Models;

namespace DevTrack.Web.Profiles
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<ApplicationUser, SettingsEditModel>()
                .ReverseMap();

            CreateMap<UpdatePassword, PasswordEditModel>()
                .ReverseMap();

            CreateMap<ProjectCreateModel, Project>()
               .ReverseMap();

            CreateMap<ProjectEditModel, Project>()
                .ReverseMap();

            CreateMap<ProjectViewModel, Project>()
                .ReverseMap();

            CreateMap<InvitationModel, Invitation>()
                .ReverseMap();

            CreateMap<InvitationResponseModel, Invitation>()
                .ReverseMap();

            CreateMap<ProjectUserEmailListModel, UserEmailList>()
                .ReverseMap();
        }
    }
}