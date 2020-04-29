using Exoft.Gamification.Api.Data.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exoft.Gamification.Api.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(i => i.Title).IsRequired();

            builder.Property(i => i.Price).IsRequired();
        }
    }
}
