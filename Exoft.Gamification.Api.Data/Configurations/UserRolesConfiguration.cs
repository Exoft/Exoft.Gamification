using Exoft.Gamification.Api.Data.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Exoft.Gamification.Api.Data.Configurations
{
    public class UserRolesConfiguration : IEntityTypeConfiguration<UserRoles>
    {
        public void Configure(EntityTypeBuilder<UserRoles> builder)
        {
            builder.Property<Guid>("UserId");
            builder.Property<Guid>("RoleId");

            builder.HasKey("UserId", "RoleId");
            builder.HasOne(ua => ua.User).WithMany(u => u.Roles).IsRequired();

            builder.HasOne(ua => ua.Role).WithMany();
        }
    }
}
