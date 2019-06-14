using AutoMapper;
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
            CreateMap<Achievement, ReadAchievementModel>()
                .ForMember(d => d.IconId, o => o.MapFrom(s => s.Icon.Id));

            CreateMap<CreateAchievementModel, ReadAchievementModel>();

            CreateMap<UpdateAchievementModel, ReadAchievementModel>();

            CreateMap<User, ReadShortUserModel>()
                .ForMember(d => d.AvatarId, o => o.MapFrom(s => s.Avatar.Id));

            CreateMap<User, ReadFullUserModel>()
                .ForMember(d => d.AvatarId, o => o.MapFrom(s => s.Avatar.Id));


            // map to entity

            CreateMap<UpdateAchievementModel, Achievement>();

            CreateMap<CreateUserModel, User>();
        }
    }
}
