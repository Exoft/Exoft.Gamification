using Exoft.Gamification.Api.Data.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Exoft.Gamification.Api.Data.Configurations
{
    public class UserAchievementConfiguration : IEntityTypeConfiguration<UserAchievement>
    {
        public void Configure(EntityTypeBuilder<UserAchievement> builder)
        {
            builder.Property<Guid>("UserId");
            builder.Property<Guid>("AchievementId");

            builder.HasKey("Id");
            builder.HasOne(ua => ua.User).WithMany(u => u.Achievements);

            builder.HasOne(ua => ua.Achievement).WithMany();
        }
    }
}
