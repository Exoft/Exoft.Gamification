using Exoft.Gamification.Api.Data.Configurations;
using Exoft.Gamification.Api.Data.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exoft.Gamification.Api.Data
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext() { }
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<UserAchievements> UserAchievements { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserAchievementsConfiguration());
            modelBuilder.ApplyConfiguration(new UserRolesConfiguration());
        }
    }
}
