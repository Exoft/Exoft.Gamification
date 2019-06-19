using AutoMapper;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Entities;

namespace Exoft.Gamification.Api.Common.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // map to model
            CreateMap<Achievement, ReadAchievementModel>();

            CreateMap<CreateAchievementModel, ReadAchievementModel>();

            CreateMap<UserAchievements, ReadAchievementModel>()
                .ForMember(s => s.AchievementId, o => o.MapFrom(d => d.Achievement.Id))
                .ForMember(s => s.UserAchievementsId, o => o.MapFrom(d => d.Id))
                .ForMember(s => s.XP, o => o.MapFrom(d => d.Achievement.XP))
                .ForMember(s => s.Name, o => o.MapFrom(d => d.Achievement.Name))
                .ForMember(s => s.Description, o => o.MapFrom(d => d.Achievement.Description))
                .ForMember(s => s.IconId, o => o.MapFrom(d => d.Achievement.IconId));

            CreateMap<UpdateAchievementModel, ReadAchievementModel>();

            CreateMap<User, ReadShortUserModel>();

            CreateMap<User, ReadFullUserModel>();
            
            CreateMap<File, FileModel>();

            CreateMap<Event, EventModel>()
                .ForMember(s => s.FirstName, o => o.MapFrom(d => d.User.FirstName))
                .ForMember(s => s.LastName, o => o.MapFrom(d => d.User.LastName))
                .ForMember(s => s.Type, o => o.MapFrom(d => d.Type.ToString()))
                .ForMember(s => s.CreatedTime, o => o.MapFrom(d => d.CreatedTime.ConvertToIso8601DateTimeUtc()));

            // map to entity

            CreateMap<UpdateAchievementModel, Achievement>();

            CreateMap<CreateUserModel, User>();
        }
    }
}
