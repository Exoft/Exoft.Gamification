using Exoft.Gamification.Api.Data.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exoft.Gamification.Api.Data.Configurations
{
    class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.HasMany(c => c.Articles).WithOne();
        }
    }
}
