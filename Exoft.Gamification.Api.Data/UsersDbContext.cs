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
        public DbSet<UserAchievement> UserAchievement { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Thank> Thanks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserAchievementConfiguration());
            modelBuilder.ApplyConfiguration(new UserRolesConfiguration());
        }
    }
}
