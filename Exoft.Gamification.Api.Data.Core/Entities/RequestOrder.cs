using System;

using Exoft.Gamification.Api.Data.Core.Helpers;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class RequestOrder : Entity
    {
        public Guid UserId { get; set; }

        public Guid OrderId { get; set; }

        public string Message { get; set; }

        public GamificationEnums.RequestStatus Status { get; set; }
    }
}
