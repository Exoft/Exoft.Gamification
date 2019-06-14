using AutoMapper;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;

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


            // map to entity

            CreateMap<UpdateAchievementModel, Achievement>();
        }
    }
}
