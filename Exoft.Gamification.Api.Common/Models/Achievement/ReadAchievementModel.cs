using System;

namespace Exoft.Gamification.Api.Common.Models.Achievement
{
    public class ReadAchievementModel
    {
        public Guid UserAchievementsId { get; set; }

        public Guid AchievementId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int XP { get; set; }

        public Guid IconId { get; set; }
    }
}
