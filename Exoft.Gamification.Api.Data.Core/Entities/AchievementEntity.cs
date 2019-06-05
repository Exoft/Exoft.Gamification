using System.Collections.Generic;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class AchievementEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int XP { get; set; }

        public string Icon { get; set; }

        public ICollection<UserAchievementsEntity> Users { get; set; }

        public AchievementEntity()
        {
            Users = new List<UserAchievementsEntity>();
        }
    }
}
