using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class UserAchievementsEntity
    {
        public int UserId { get; set; }
        public UserEntity User { get; set; }

        public int AchievementId { get; set; }
        public AchievementEntity Achievement { get; set; }
    }
}
