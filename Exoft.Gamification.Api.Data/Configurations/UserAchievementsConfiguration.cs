using Exoft.Gamification.Api.Data.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Data.Configurations
{
    public class UserAchievementsConfiguration : IEntityTypeConfiguration<UserAchievements>
    {
        public void Configure(EntityTypeBuilder<UserAchievements> builder)
        {
            builder.HasKey(ua => new { ua.UserId, ua.AchievementId });
            builder.HasOne(ua => ua.User).WithMany(u => u.Achievements).HasForeignKey(ua => ua.UserId);
        }
    }
}
