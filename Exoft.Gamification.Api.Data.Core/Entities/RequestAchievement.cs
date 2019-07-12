using System;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class RequestAchievement : Entity
    {
        public Guid UserId { get; set; }

        public Guid AchievementId { get; set; }

        public string Message { get; set; }
    }
}
