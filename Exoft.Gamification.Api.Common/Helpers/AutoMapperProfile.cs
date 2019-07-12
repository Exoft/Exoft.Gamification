using AutoMapper;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Common.Models.Thank;
using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Entities;
using System.Linq;

namespace Exoft.Gamification.Api.Common.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // map to model
            CreateMap<Achievement, ReadAchievementModel>();

            CreateMap<CreateAchievementModel, ReadAchievementModel>();

            CreateMap<UserAchievement, ReadUserAchievementModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id))
                .ForMember(s => s.AchievementId, o => o.MapFrom(d => d.Achievement.Id))
                .ForMember(s => s.XP, o => o.MapFrom(d => d.Achievement.XP))
                .ForMember(s => s.Name, o => o.MapFrom(d => d.Achievement.Name))
                .ForMember(s => s.Description, o => o.MapFrom(d => d.Achievement.Description))
                .ForMember(s => s.IconId, o => o.MapFrom(d => d.Achievement.IconId))
                .ForMember(s => s.AddedTime, o => o.MapFrom(d => d.AddedTime.ConvertToIso8601DateTimeUtc()));

            CreateMap<UserAchievement, ReadAchievementModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Achievement.Id))
                .ForMember(s => s.Description, o => o.MapFrom(d => d.Achievement.Description))
                .ForMember(s => s.Name, o => o.MapFrom(d => d.Achievement.Name))
                .ForMember(s => s.IconId, o => o.MapFrom(d => d.Achievement.IconId))
                .ForMember(s => s.XP, o => o.MapFrom(d => d.Achievement.XP));

            CreateMap<UpdateAchievementModel, ReadAchievementModel>();

            CreateMap<User, ReadShortUserModel>();

            CreateMap<User, ReadFullUserModel>();
            
            CreateMap<File, FileModel>();

            CreateMap<Event, EventModel>()
                .ForMember(s => s.FirstName, o => o.MapFrom(d => d.User.FirstName))
                .ForMember(s => s.LastName, o => o.MapFrom(d => d.User.LastName))
                .ForMember(s => s.AvatarId, o => o.MapFrom(d => d.User.AvatarId))
                .ForMember(s => s.Type, o => o.MapFrom(d => d.Type.ToString()))
                .ForMember(s => s.CreatedTime, o => o.MapFrom(d => d.CreatedTime.ConvertToIso8601DateTimeUtc()));
            
            CreateMap<User, JwtTokenModel>()
                .ForMember(s => s.Roles, o => o.MapFrom(d => d.Roles.Select(i => i.Role.Name)));
            
            CreateMap<Thank, ReadThankModel>()
                .ForMember(s => s.FirstName, o => o.MapFrom(d => d.FromUser.FirstName))
                .ForMember(s => s.LastName, o => o.MapFrom(d => d.FromUser.LastName))
                .ForMember(s => s.AvatarId, o => o.MapFrom(d => d.FromUser.AvatarId));

            // map to entity

            CreateMap<UpdateAchievementModel, Achievement>();

            CreateMap<CreateUserModel, User>();

            CreateMap<CreateThankModel, Thank>();

            CreateMap<RequestAchievementModel, RequestAchievement>();
        }
    }
}
