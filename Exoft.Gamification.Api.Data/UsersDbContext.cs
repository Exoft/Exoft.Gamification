using Exoft.Gamification.Api.Data.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exoft.Gamification.Api.Data
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        public UsersDbContext()
        {
        }
        
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<AchievementEntity> Achievements { get; set; }
        public DbSet<UserAchievementsEntity> UserAchievements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAchievementsEntity>().HasKey(ua => new { ua.UserId, ua.AchievementId });
            modelBuilder.Entity<UserAchievementsEntity>().HasOne(ua => ua.User).WithMany(u => u.Achievements).HasForeignKey(u => u.UserId);
            modelBuilder.Entity<UserAchievementsEntity>().HasOne(ua => ua.Achievement).WithMany(d => d.Users).HasForeignKey(pd => pd.AchievementId);

            modelBuilder.Entity<UserAchievementsEntity>().ToTable("UserAchievements");
        }
    }
}
