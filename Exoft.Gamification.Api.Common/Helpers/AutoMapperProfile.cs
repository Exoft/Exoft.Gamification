using AutoMapper;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;

namespace Exoft.Gamification.Api.Common.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Achievement, OutAchievementModel>()
                .ForMember(d => d.IconId, o => o.MapFrom(s => s.Icon.Id));

            CreateMap<InAchievementModel, Achievement>();
        }
    }
}
