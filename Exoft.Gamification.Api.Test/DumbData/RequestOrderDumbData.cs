using Exoft.Gamification.Api.Common.Models.RequestOrder;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Test.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using static Exoft.Gamification.Api.Data.Core.Helpers.GamificationEnums;

namespace Exoft.Gamification.Api.Test.DumbData
{
    public class RequestOrderDumbData
    {
        public static RequestOrder GetRandomEntity()
        {
            var rnd = new Random();
            return new RequestOrder
            {
                Message = RandomHelper.GetRandomString(20),
                Status = (RequestStatus)rnd.Next(Enum.GetNames(typeof(RequestStatus)).Length)
            };
        }

        public static List<RequestOrder> GetRandomEntities(int number)
        {
            var list = new List<RequestOrder>();
            for (int i = 0; i < number; i++)
            {
                list.Add(GetRandomEntity());
            }
            return list;
        }

        public static List<RequestOrder> GetEntities(IEnumerable<User> users, IEnumerable<Order> orders)
        {
            var list = new List<RequestOrder>();
            for (int i = 0; i < users.Count(); i++)
            {
                list.Add(GetEntity( users.ElementAt(i).Id, orders.ElementAt(i).Id));
            }
            return list;
        }

        public static RequestOrder GetEntity(Guid userId, Guid orderId)
        {
            return new RequestOrder
            {
                UserId = userId,
                OrderId = orderId
            };
        }

        public static CreateRequestOrderModel GetCreateRequestOrderModel()
        {
            return new CreateRequestOrderModel
            {
                Message = RandomHelper.GetRandomString(20)
            };
        }
    }
}
