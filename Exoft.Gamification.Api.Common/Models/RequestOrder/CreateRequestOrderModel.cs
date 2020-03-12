using System;

namespace Exoft.Gamification.Api.Common.Models.RequestOrder
{
    public class CreateRequestOrderModel
    {
        public string Message { get; set; }

        public Guid OrderId { get; set; }
    }
}
