using System;

namespace Exoft.Gamification.Api.Common.Models.RequestOrder
{
    public class ReadRequestOrderModel
    {
        public Guid Id { get; set; }

        public string Message { get; set; }

        public string OrderName { get; set; }

        public string UserName { get; set; }

        public Guid OrderId { get; set; }

        public Guid UserId { get; set; }
    }
}
