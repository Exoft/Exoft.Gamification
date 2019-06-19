namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class UserAchievements : Entity
    {
        public User User { get; set; }

        public Achievement Achievement { get; set; }
    }
}
