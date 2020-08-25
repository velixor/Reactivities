using System.Linq;
using Application.Dtos;
using AutoMapper;
using Domain;

namespace Application.MapperProfiles
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            Activity();
            User();
        }

        private void Activity()
        {
            CreateMap<Activity, ActivityDto>()
                .ForMember(x => x.Attendees, opt => opt.MapFrom(x => x.UserActivities));

            CreateMap<UserActivity, AttendeeDto>()
                .ForMember(x => x.DisplayName, opt => opt.MapFrom(x => x.AppUser.DisplayName))
                .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.AppUser.UserName));
        }

        private void User()
        {
            CreateMap<AppUser, Profiles.Profile>()
                .ForMember(x => x.Image, opt =>
                {
                    opt.PreCondition(user => user.Photos.Any(ph => ph.IsMain));
                    opt.MapFrom(x => x.Photos.FirstOrDefault(ph => ph.IsMain).Url);
                });
        }
    }
}