using Exoft.Gamification.Api.Data.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Exoft.Gamification.Api.Data.Configurations
{
    public class UserAchievementsConfiguration : IEntityTypeConfiguration<UserAchievements>
    {
        public void Configure(EntityTypeBuilder<UserAchievements> builder)
        {
            builder.Property<Guid>("UserId");
            builder.Property<Guid>("AchievementId");

            builder.HasKey("UserId", "AchievementId");
            builder.HasOne(ua => ua.User).WithMany(u => u.Achievements);

            builder.HasOne(ua => ua.Achievement).WithMany();
        }
    }
}
