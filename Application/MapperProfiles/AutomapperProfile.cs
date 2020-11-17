using System.Linq;
using Application.Comments;
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
            Comment();
        }

        private void Activity()
        {
            CreateMap<Activity, ActivityDto>()
                .ForMember(d => d.Attendees, opt => opt.MapFrom(s => s.UserActivities));

            CreateMap<UserActivity, AttendeeDto>()
                .ForMember(d => d.DisplayName, opt => opt.MapFrom(s => s.AppUser.DisplayName))
                .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.AppUser.UserName))
                .ForMember(d => d.Image, opt => opt.MapFrom(s => s.AppUser.MainPhoto.Url));
        }

        private void User()
        {
            CreateMap<AppUser, Profiles.Profile>()
                .ForMember(d => d.Image, opt => opt.MapFrom(s => s.Photos.FirstOrDefault(ph => ph.IsMain).Url));
        }

        private void Comment()
        {
            CreateMap<Comment, CommentDto>()
                .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.DisplayName, opt => opt.MapFrom(s => s.Author.DisplayName))
                .ForMember(d => d.Image, opt => opt.MapFrom(s => s.Author.Photos.FirstOrDefault(ph => ph.IsMain).Url));
        }
    }
}