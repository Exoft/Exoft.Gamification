using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exoft.Gamification.Api.Data.Seeds
{
    public class ContextInitializer
    {
        public static void Initialize(UsersDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                context.Users.Add(new Core.Entities.UserEntity()
                {
                    //Id = 0,
                    FirstName = "Ostap",
                    LastName = "Roik",
                    Email = "ostap2308@gmail.com",
                    UserName = "OstapRoik",
                    Password = "password",
                    Status = "Status bla bla bla",
                    Avatar = null,
                    XP = 10
                });
                context.Users.Add(new Core.Entities.UserEntity()
                {
                    //Id = 1,
                    FirstName = "Tanya",
                    LastName = "Gogina",
                    Email = "tanya@gmail.com",
                    UserName = "TanyaGogina",
                    Password = "password",
                    Status = "Status 123",
                    Avatar = null,
                    XP = 40
                });
                context.SaveChanges();
            }

            if (!context.Achievements.Any())
            {
                context.Achievements.Add(new Core.Entities.AchievementEntity()
                {
                    //Id = 0,
                    Name = "Welcome",
                    Description = "A newcomer to the team",
                    Icon = null,
                    XP = 10
                });
                context.Achievements.Add(new Core.Entities.AchievementEntity()
                {
                    //Id = 1,
                    Name = "1 year",
                    Description = "1 year in company",
                    Icon = null,
                    XP = 30
                });
                context.SaveChanges();
            }

            if (!context.UserAchievements.Any())
            {
                context.UserAchievements.Add(new Core.Entities.UserAchievementsEntity()
                {
                    UserId = 1,
                    AchievementId = 1
                });
                context.UserAchievements.Add(new Core.Entities.UserAchievementsEntity()
                {
                    UserId = 2,
                    AchievementId = 1
                });
                context.UserAchievements.Add(new Core.Entities.UserAchievementsEntity()
                {
                    UserId = 2,
                    AchievementId = 2
                });
                context.SaveChanges();
            }
        }
    }
}
