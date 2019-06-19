using Exoft.Gamification.Api.Data.Core.Helpers;
using System.Linq;

namespace Exoft.Gamification.Api.Data.Seeds
{
    public class ContextInitializer
    {
        public static void Initialize(UsersDbContext context)
        {
            context.Database.EnsureCreated();

            var user1 = new Core.Entities.User()
            {
                FirstName = "Ostap",
                LastName = "Roik",
                Email = "ostap2308@gmail.com",
                UserName = "OstapRoik",
                Password = "password",
                Status = "Status bla bla bla",
                XP = 10
            };
            var user2 = new Core.Entities.User()
            {
                FirstName = "Tanya",
                LastName = "Gogina",
                Email = "tanya@gmail.com",
                UserName = "TanyaGogina",
                Password = "password",
                Status = "Status 123",
                XP = 40
            };
            var achievement1 = new Core.Entities.Achievement()
            {
                Name = "Welcome",
                Description = "A newcomer to the team",
                XP = 10
            };
            var achievement2 = new Core.Entities.Achievement()
            {
                Name = "1 year",
                Description = "1 year in company",
                XP = 30
            };
            var role1 = new Core.Entities.Role()
            {
                Name = "Admin"
            };
            var role2 = new Core.Entities.Role()
            {
                Name = "User"
            };
            var event1 = new Core.Entities.Event()
            {
                Description = "First",
                Type = GamificationEnums.EventType.Race,
                User = user1
            };
            var event2 = new Core.Entities.Event()
            {
                Description = "Second",
                Type = GamificationEnums.EventType.Records,
                User = user1
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

            if (!context.Roles.Any())
            {
                context.Roles.Add(role1);
                context.Roles.Add(role2);
                context.SaveChanges();
            }

            if (!context.UserAchievement.Any())
            {
                context.UserAchievement.Add(new Core.Entities.UserAchievement()
                {
                    User = user1,
                    Achievement = achievement1,
                    Comment = "Some text"
                });
                context.UserAchievement.Add(new Core.Entities.UserAchievement()
                {
                    User = user2,
                    Achievement = achievement1,
                    Comment = "Some text"
                });
                context.UserAchievement.Add(new Core.Entities.UserAchievement()
                {
                    User = user2,
                    Achievement = achievement2,
                    Comment = "Some text"
                });
                context.SaveChanges();
            }

            if (!context.UserRoles.Any())
            {
                context.UserRoles.Add(new Core.Entities.UserRoles()
                {
                    User = user1,
                    Role = role1
                });
                context.UserRoles.Add(new Core.Entities.UserRoles()
                {
                    User = user2,
                    Role = role2
                });
                context.SaveChanges();
            }

            if(!context.Events.Any())
            {
                context.Events.Add(event1);
                context.Events.Add(event2);
                context.SaveChanges();
            }
        }
    }
}
