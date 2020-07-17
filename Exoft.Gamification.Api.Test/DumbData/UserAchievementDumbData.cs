using Exoft.Gamification.Api.Data.Core.Entities;
using System.Collections.Generic;

namespace Exoft.Gamification.Api.Test.DumbData
{
    public static class UserAchievementDumbData
    {
        public static UserAchievement GetRandomEntity(User user = null, Achievement achievement = null)
        {
            return new UserAchievement
            {
                User = user ?? UserDumbData.GetRandomEntity(),
                Achievement = achievement ?? AchievementDumbData.GetRandomEntity()
            };
        }

        public static List<UserAchievement> GetRandomEntities(int number, User user = null, Achievement achievement = null)
        {
            var list = new List<UserAchievement>();
            for (int i = 0; i < number; i++)
            {
                list.Add(GetRandomEntity(user, achievement));
            }
            return list;
        }
    }
}
