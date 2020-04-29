using System;

namespace Exoft.Gamification.Api.Common.Models.RequestAchievement
{
    public class CreateRequestAchievementModel
    {
        public string Message { get; set; }

        public Guid AchievementId { get; set; }
    }
}
