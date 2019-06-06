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

            var user1 = new Core.Entities.User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Ostap",
                LastName = "Roik",
                Email = "ostap2308@gmail.com",
                UserName = "OstapRoik",
                Password = "password",
                Status = "Status bla bla bla",
                Avatar = null,
                XP = 10
            };
            var user2 = new Core.Entities.User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Tanya",
                LastName = "Gogina",
                Email = "tanya@gmail.com",
                UserName = "TanyaGogina",
                Password = "password",
                Status = "Status 123",
                Avatar = null,
                XP = 40
            };
            var achievement1 = new Core.Entities.Achievement()
            {
                Id = Guid.NewGuid(),
                Name = "Welcome",
                Description = "A newcomer to the team",
                Icon = null,
                XP = 10
            };
            var achievement2 = new Core.Entities.Achievement()
            {
                Id = Guid.NewGuid(),
                Name = "1 year",
                Description = "1 year in company",
                Icon = null,
                XP = 30
            };

            if (!context.Users.Any())
            {
                context.Users.Add(user1);
                context.Users.Add(user2);
                context.SaveChanges();
            }

            if (!context.Achievements.Any())
            {
                context.Achievements.Add(achievement1);
                context.Achievements.Add(achievement2);
                context.SaveChanges();
            }

            if (!context.UserAchievements.Any())
            {
                context.UserAchievements.Add(new Core.Entities.UserAchievements()
                {
                    User = user1,
                    UserId = user1.Id,
                    Achievement = achievement1,
                    AchievementId = achievement1.Id
                });
                context.UserAchievements.Add(new Core.Entities.UserAchievements()
                {
                    User = user2,
                    UserId = user2.Id,
                    Achievement = achievement1,
                    AchievementId = achievement1.Id
                });
                context.UserAchievements.Add(new Core.Entities.UserAchievements()
                {
                    User = user2,
                    UserId = user2.Id,
                    Achievement = achievement2,
                    AchievementId = achievement2.Id
                });
                context.SaveChanges();
            }
        }
    }
}
