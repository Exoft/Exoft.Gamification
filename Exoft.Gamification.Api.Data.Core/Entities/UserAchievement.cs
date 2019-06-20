using System;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class UserAchievement : Entity
    {
        public UserAchievement()
        {
            AddedTime = DateTime.UtcNow;
        }

        public User User { get; set; }

        public Achievement Achievement { get; set; }

        public DateTime AddedTime { get; private set; }
    }
}
