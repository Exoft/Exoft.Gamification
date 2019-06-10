using AutoMapper;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exoft.Gamification.Api.Common.Helpers
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            // map to model

            CreateMap<User, UserModel>()
                .ForMember(i => i.Roles,
                                m => m.MapFrom(
                                    p => p.Roles.Select(d => new RoleModel()
                                    {
                                        Id = d.Role.Id,
                                        Text = d.Role.Text
                                    }).ToList()))
                .ForMember(i => i.Achievements,
                                m => m.MapFrom(
                                    p => p.Achievements.Select(d => new AchievementModel()
                                    {
                                        Id = d.Achievement.Id,
                                        Description = d.Achievement.Description,
                                        Icon = d.Achievement.Icon,
                                        Name = d.Achievement.Name,
                                        XP = d.Achievement.XP
                                    }).ToList()));

            // map to entity

            CreateMap<NewUserModel, User>();

            CreateMap<NewAchievementModel, Achievement>();

            CreateMap<UpdateAchievementModel, Achievement>();
        }
    }
}
