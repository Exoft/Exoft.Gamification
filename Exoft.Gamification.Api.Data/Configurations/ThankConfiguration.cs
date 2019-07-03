using Exoft.Gamification.Api.Data.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exoft.Gamification.Api.Data.Configurations
{
    public class ThankConfiguration : IEntityTypeConfiguration<Thank>
    {
        public void Configure(EntityTypeBuilder<Thank> builder)
        {
            builder.HasOne(i => i.FromUser).WithMany().OnDelete(DeleteBehavior.Restrict).IsRequired();
            
            builder.HasOne<User>().WithMany().HasForeignKey(i => i.ToUserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
