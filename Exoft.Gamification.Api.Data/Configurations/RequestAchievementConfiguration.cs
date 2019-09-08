using Exoft.Gamification.Api.Data.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exoft.Gamification.Api.Data.Configurations
{
    public class RequestAchievementConfiguration : IEntityTypeConfiguration<RequestAchievement>
    {
        public void Configure(EntityTypeBuilder<RequestAchievement> builder)
        {
            //builder.HasOne<User>().WithMany().HasForeignKey(i => i.UserId);

            //builder.HasOne<Achievement>().WithMany().HasForeignKey(i => i.AchievementId);
        }
    }
}
