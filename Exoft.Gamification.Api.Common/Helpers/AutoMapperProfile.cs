using AutoMapper;
using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Data.Core.Entities;

namespace Exoft.Gamification.Api.Common.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Achievement, ReadAchievementModel>()
                .ForMember(d => d.IconId, o => o.MapFrom(s => s.Icon.Id));

            CreateMap<CreateAchievementModel, Achievement>();

            CreateMap<UpdateAchievementModel, Achievement>();
        }
    }
}
