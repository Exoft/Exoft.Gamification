using Exoft.Gamification.Api.Common.Models.Category;
using Exoft.Gamification.Api.Common.Models.Order;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Test.TestData;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Test.DumbData
{
    public class OrderDumbData
    {
        public static Order GetRandomEntity()
        {
            var order = new Order
            {
                Description = RandomHelper.GetRandomString(200),
                Popularity = RandomHelper.GetRandomNumber(),
                Price = RandomHelper.GetRandomNumber(),
                Title = RandomHelper.GetRandomString(20),
                Categories = new List<OrderCategory>()
            };
            order.Categories.Add(new OrderCategory 
            { 
                Order = order, 
                Category = CategoryDumbData.GetRandomEntity() 
            });
            order.Categories.Add(new OrderCategory
            { 
                Order = order, 
                Category = CategoryDumbData.GetRandomEntity() 
            });
            return order;
        }

        public static List<Order> GetRandomEntities(int number)
        {
            var list = new List<Order>();
            for (int i = 0; i < number; i++)
            {
                list.Add(GetRandomEntity());
            }
            return list;
        }

        public static ReadOrderModel GetReadAchievementModel(CreateOrderModel model)
        {
            return new ReadOrderModel
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Description = model.Description,
                Price = model.Price
            };
        }

        public static CreateOrderModel GetCreateOrderModel()
        {
            return new CreateOrderModel
            {
                CategoryIds = new List<Guid>
                {
                    Guid.NewGuid(),
                    Guid.NewGuid()
                },
                Description = RandomHelper.GetRandomString(200),
                Price = RandomHelper.GetRandomNumber(),
                Title = RandomHelper.GetRandomString(20)
            };
        }

        public static UpdateOrderModel GetUpdateOrderModel()
        {
            return new UpdateOrderModel
            {
                CategoryIds = new List<Guid>
                {
                    Guid.NewGuid(),
                    Guid.NewGuid()
                },
                Description = RandomHelper.GetRandomString(200),
                Price = RandomHelper.GetRandomNumber(),
                Title = RandomHelper.GetRandomString(20)
            };
        }

        public static Order GetEntity(UpdateOrderModel model)
        {
            return new Order
            {
                Title = model.Title,
                Description = model.Description,
                Price = model.Price
            };
        }
    }
}
