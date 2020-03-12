using Exoft.Gamification.Api.Data.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exoft.Gamification.Api.Data.Configurations
{
    public class RequestOrderConfiguration : IEntityTypeConfiguration<RequestOrder>
    {
        public void Configure(EntityTypeBuilder<RequestOrder> builder)
        {
            builder.HasOne<User>().WithMany().HasForeignKey(i => i.UserId);

            builder.HasOne<Order>().WithMany().HasForeignKey(i => i.OrderId);
        }
    }
}
