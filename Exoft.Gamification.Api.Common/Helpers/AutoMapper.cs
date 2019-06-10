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
                .ForMember(i => i.AvatarId, m => m.MapFrom(p => p.Avatar.Id));

            // map to entity

            CreateMap<NewUserModel, User>();
        }
    }
}
