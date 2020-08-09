using Application.Dtos;
using AutoMapper;
using Domain;

namespace Application.MapperProfiles
{
    public class ActivityProfile : Profile
    {
        public ActivityProfile()
        {
            CreateMap<Activity, ActivityDto>()
                .ForMember(x => x.Attendees, opt => opt.MapFrom(x => x.UserActivities));

            CreateMap<UserActivity, AttendeeDto>()
                .ForMember(x => x.DisplayName, opt => opt.MapFrom(x => x.AppUser.DisplayName))
                .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.AppUser.UserName));
        }
    }
}