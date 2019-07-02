using AutoMapper;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Entities;
using System;

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
                .ForMember(s => s.Type, o => o.MapFrom(d => d.Type.ToString()))
                .ForMember(s => s.CreatedTime, o => o.MapFrom(d => d.CreatedTime.ConvertToIso8601DateTimeUtc()));

            CreateMap<User, JwtTokenModel>();

            // map to entity

            CreateMap<UpdateAchievementModel, Achievement>();

            CreateMap<CreateUserModel, User>();
        }
    }
}
