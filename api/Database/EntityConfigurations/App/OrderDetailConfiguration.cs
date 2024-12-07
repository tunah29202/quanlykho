using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("a_order_detail", "public");

            builder.Property(t => t.order_id).IsRequired();
            builder.Property(t => t.quantity).IsRequired();

            builder
            .HasOne(x => x.order)
            .WithMany(y => y.order_details)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.order_id)
            .OnDelete(DeleteBehavior.Restrict);

            builder
            .HasOne(x => x.product)
            .WithMany(y => y.order_details)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.product_id)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}