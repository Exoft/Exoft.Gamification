﻿using AutoMapper;
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

            CreateMap<UpdateAchievementModel, ReadAchievementModel>();

            CreateMap<User, ReadShortUserModel>();

            CreateMap<User, ReadFullUserModel>();
            
            CreateMap<File, FileModel>();

            CreateMap<Event, EventModel>()
                .ForMember(s => s.FirstName, o => o.MapFrom(d => d.User.FirstName))
                .ForMember(s => s.LastName, o => o.MapFrom(d => d.User.LastName))
                .ForMember(s => s.Type, o => o.MapFrom(d => d.Type.ToString()))
                .ForMember(s => s.Time, o => o.MapFrom(d => d.Time.ToString("o")));

            // map to entity

            CreateMap<UpdateAchievementModel, Achievement>();

            CreateMap<CreateUserModel, User>();
        }
    }
}
