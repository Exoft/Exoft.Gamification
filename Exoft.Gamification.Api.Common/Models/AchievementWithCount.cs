using System;

namespace Exoft.Gamification.Api.Common.Models
{
    public class AchievementWithCount
    {
        public Guid AchievementId { get; set; }

        public int Count { get; set; }
    }
}
