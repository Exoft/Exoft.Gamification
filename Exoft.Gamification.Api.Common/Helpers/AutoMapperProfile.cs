using System.Linq;

using AutoMapper;

using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Common.Models.Category;
using Exoft.Gamification.Api.Common.Models.Order;
using Exoft.Gamification.Api.Common.Models.Thank;
using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Entities;

namespace Exoft.Gamification.Api.Common.Helpers
{
    using Exoft.Gamification.Api.Common.Models.RequestAchievement;
    using Exoft.Gamification.Api.Common.Models.RequestOrder;

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
                .ForMember(s => s.AddedTime, o => o.MapFrom(d => d.AddedTime));

            CreateMap<UserAchievement, ReadAchievementModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Achievement.Id))
                .ForMember(s => s.Description, o => o.MapFrom(d => d.Achievement.Description))
                .ForMember(s => s.Name, o => o.MapFrom(d => d.Achievement.Name))
                .ForMember(s => s.IconId, o => o.MapFrom(d => d.Achievement.IconId))
                .ForMember(s => s.XP, o => o.MapFrom(d => d.Achievement.XP));

            CreateMap<UpdateAchievementModel, ReadAchievementModel>();

            CreateMap<User, ReadShortUserModel>();

            CreateMap<User, ReadFullUserModel>()
                .ForMember(s => s.Roles, o => o.MapFrom(d => d.Roles.Select(i => i.Role.Name)));

            CreateMap<File, FileModel>();

            CreateMap<Event, EventModel>()
                .ForMember(s => s.FirstName, o => o.MapFrom(d => d.User.FirstName))
                .ForMember(s => s.LastName, o => o.MapFrom(d => d.User.LastName))
                .ForMember(s => s.AvatarId, o => o.MapFrom(d => d.User.AvatarId))
                .ForMember(s => s.Type, o => o.MapFrom(d => d.Type.ToString()))
                .ForMember(s => s.CreatedTime, o => o.MapFrom(d => d.CreatedTime));

            CreateMap<User, JwtTokenModel>()
                .ForMember(s => s.Roles, o => o.MapFrom(d => d.Roles.Select(i => i.Role.Name)));

            CreateMap<Thank, ReadThankModel>()
                .ForMember(s => s.FirstName, o => o.MapFrom(d => d.FromUser.FirstName))
                .ForMember(s => s.LastName, o => o.MapFrom(d => d.FromUser.LastName))
                .ForMember(s => s.AvatarId, o => o.MapFrom(d => d.FromUser.AvatarId))
                .ForMember(s => s.UserId, o => o.MapFrom(d => d.FromUser.Id));

            CreateMap<Order, ReadOrderModel>()
                .ConvertUsing<ReadOrderModelConverter>();

            CreateMap<Category, ReadCategoryModel>();

            CreateMap<RequestOrder, ReadRequestOrderModel>();

            // map to entity
            CreateMap<UpdateAchievementModel, Achievement>();

            CreateMap<CreateUserModel, User>()
                .ForMember(s => s.Roles, o => o.Ignore())
                .ForMember(s => s.Achievements, o => o.Ignore());

            CreateMap<CreateThankModel, Thank>();

            CreateMap<CreateRequestAchievementModel, RequestAchievement>();

            CreateMap<CreateOrderModel, Order>();

            CreateMap<CreateCategoryModel, Category>();

            CreateMap<CreateRequestOrderModel, RequestOrder>();
        }

        public class ReadOrderModelConverter : ITypeConverter<Order, ReadOrderModel>
        {
            public ReadOrderModel Convert(Order source, ReadOrderModel destination, ResolutionContext context)
            {
                return new ReadOrderModel
                {
                    Title = source.Title,
                    Description = source.Description,
                    Price = source.Price,
                    Popularity = source.Popularity,
                    IconId = source.IconId,
                    Categories = source.Categories
                        .Select(i => context.Mapper.Map<ReadCategoryModel>(i.Category))
                        .ToList()
                };
            }
        }
    }
}
