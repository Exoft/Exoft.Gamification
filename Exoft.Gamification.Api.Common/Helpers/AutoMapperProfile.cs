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

            CreateMap<ReturnPagingInfo<Achievement>, ReturnPagingModel<ReadAchievementModel>>()
                .ForMember(d => d.Data, o => o.MapFrom(s => s.Data));

            CreateMap<CreateAchievementModel, ReadAchievementModel>();

            CreateMap<UpdateAchievementModel, ReadAchievementModel>();


            // map to entity

            CreateMap<UpdateAchievementModel, Achievement>();

            CreateMap<InputPagingModel, PagingInfo>()
                .ForMember(d => d.CurrentPage, m => m.MapFrom(s => s.PageNumber));
        }
    }
}
