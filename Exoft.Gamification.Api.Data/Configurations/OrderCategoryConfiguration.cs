using System;

using Exoft.Gamification.Api.Data.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exoft.Gamification.Api.Data.Configurations
{
    public class OrderCategoryConfiguration : IEntityTypeConfiguration<OrderCategory>
    {
        public void Configure(EntityTypeBuilder<OrderCategory> builder)
        {
            builder.Property<Guid>("OrderId");
            builder.Property<Guid>("CategoryId");

            builder.HasKey("Id");
            builder.HasOne(ua => ua.Order).WithMany(u => u.Categories);

            builder.HasOne(ua => ua.Category).WithMany();
        }
    }
}
