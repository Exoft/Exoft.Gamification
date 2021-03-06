﻿using System.Reflection;
using Exoft.Gamification.Api.Data.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exoft.Gamification.Api.Data
{
    public class GamificationDbContext : DbContext
    {
        public GamificationDbContext() { }

        public GamificationDbContext(DbContextOptions<GamificationDbContext> options) : base(options) { }


        public DbSet<User> Users { get; set; }

        public DbSet<Achievement> Achievements { get; set; }

        public DbSet<UserAchievement> UserAchievement { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRoles> UserRoles { get; set; }

        public DbSet<File> Files { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Thank> Thanks { get; set; }

        public DbSet<RequestAchievement> RequestAchievements { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<OrderCategory> OrderCategory { get; set; }

        public DbSet<RequestOrder> RequestOrders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
