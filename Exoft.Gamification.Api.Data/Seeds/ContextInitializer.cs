using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using File = Exoft.Gamification.Api.Data.Core.Entities.File;

namespace Exoft.Gamification.Api.Data.Seeds
{
    public class ContextInitializer
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly GamificationDbContext _context;

        public ContextInitializer(IPasswordHasher passwordHasher, GamificationDbContext context)
        {
            _passwordHasher = passwordHasher;
            _context = context;
        }

        public async Task InitializeAsync()
        {
            await _context.Database.MigrateAsync();

            var user1 = new User
            {
                FirstName = "Admin",
                LastName = "Exoft",
                Email = "admin@exoft.net",
                UserName = "AdminExoft",
                XP = 30,
                Password = _passwordHasher.GetHash("Pa$$word123"),
                Status = "Status bla bla bla"
            };
            var user2 = new User
            {
                FirstName = "Tanya",
                LastName = "Gogina",
                Email = "tanyagermain23@gmail.com",
                UserName = "TanyaGogina",
                XP = 40,
                Password = _passwordHasher.GetHash("Pa$$word123"),
                Status = "Status 123"
            };
            var user3 = new User
            {
                FirstName = "Ostap",
                LastName = "Roik",
                Email = "ostap2308@gmail.com",
                UserName = "OstapRoik",
                XP = 0,
                Password = _passwordHasher.GetHash("Pa$$word123"),
                Status = "Status 123"
            };
            var achievement1 = new Achievement
            {
                Name = "Welcome",
                Description = "A newcomer to the team",
                XP = 10
            };
            var achievement2 = new Achievement
            {
                Name = "1 year",
                Description = "1 year in company",
                XP = 30
            };
            var role1 = new Role
            {
                Name = "Admin"
            };
            var role2 = new Role
            {
                Name = "User"
            };
            var event1 = new Event
            {
                Description = "First",
                Type = GamificationEnums.EventType.Race,
                User = user1
            };
            var event2 = new Event
            {
                Description = "Second",
                Type = GamificationEnums.EventType.Records,
                User = user1
            };
            var event3 = new Event
            {
                Description = "Third",
                Type = GamificationEnums.EventType.Upload,
                User = user2
            };

            if (!await _context.Users.AnyAsync())
            {
                await _context.Users.AddAsync(user1);
                await _context.Users.AddAsync(user2);
                await _context.SaveChangesAsync();
            }

            if (!await _context.Achievements.AnyAsync())
            {
                await _context.Achievements.AddAsync(achievement1);
                await _context.Achievements.AddAsync(achievement2);
                await _context.SaveChangesAsync();
            }

            if (!await _context.Roles.AnyAsync())
            {
                await _context.Roles.AddAsync(role1);
                await _context.Roles.AddAsync(role2);
                await _context.SaveChangesAsync();
            }

            if (!await _context.UserAchievement.AnyAsync())
            {
                await _context.UserAchievement.AddAsync(new UserAchievement
                {
                    User = user1,
                    Achievement = achievement2
                });
                await _context.UserAchievement.AddAsync(new UserAchievement
                {
                    User = user2,
                    Achievement = achievement1
                });
                await _context.UserAchievement.AddAsync(new UserAchievement
                {
                    User = user2,
                    Achievement = achievement2
                });
                await _context.SaveChangesAsync();
            }

            if (!await _context.UserRoles.AnyAsync())
            {
                await _context.UserRoles.AddAsync(new UserRoles
                {
                    User = user1,
                    Role = role1
                });
                await _context.UserRoles.AddAsync(new UserRoles
                {
                    User = user2,
                    Role = role2
                });
                await _context.UserRoles.AddAsync(new UserRoles
                {
                    User = user3,
                    Role = role2
                });
                await _context.SaveChangesAsync();
            }

            if (!await _context.Events.AnyAsync())
            {
                await _context.Events.AddAsync(event1);
                await _context.Events.AddAsync(event2);
                await _context.Events.AddAsync(event3);
                await _context.SaveChangesAsync();
            }

            if (!await _context.Categories.AnyAsync())
            {
                var bytes = Convert.FromBase64String(
                    "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");

                var file = new File
                {
                    ContentType = "image/png",
                    Data = bytes
                };
                await _context.Files.AddAsync(file);
                await _context.SaveChangesAsync();

                await _context.Categories.AddRangeAsync(new List<Category>
                {
                    new Category
                    {
                        Name = "Apple",
                        IconId = file.Id
                    },
                    new Category
                    {
                        Name = "Android",
                        IconId = file.Id
                    },
                    new Category
                    {
                        Name = "Health",
                        IconId = file.Id
                    },
                    new Category
                    {
                        Name = "Food",
                        IconId = file.Id
                    },
                    new Category
                    {
                        Name = "Man",
                        IconId = file.Id
                    },
                    new Category
                    {
                        Name = "Woman",
                        IconId = file.Id
                    },
                    new Category
                    {
                        Name = "Car",
                        IconId = file.Id
                    },
                    new Category
                    {
                        Name = "Sport",
                        IconId = file.Id
                    },
                    new Category
                    {
                        Name = "Games",
                        IconId = file.Id
                    },
                    new Category
                    {
                        Name = "Puzzle",
                        IconId = file.Id
                    },
                    new Category
                    {
                        Name = "Animal",
                        IconId = file.Id
                    },
                    new Category
                    {
                        Name = "Steam",
                        IconId = file.Id
                    }
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}
