using System;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class UserAchievement : Entity
    {
        public UserAchievement()
        {
            AddedTime = DateTime.Now;   
        }

        public User User { get; set; }

        public Achievement Achievement { get; set; }

        public DateTime AddedTime { get; private set; }

        public string Comment { get; set; }
    }
}
