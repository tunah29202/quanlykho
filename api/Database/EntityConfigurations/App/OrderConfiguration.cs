using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("a_order", "public");

            builder.Property(t => t.order_no).HasMaxLength(20).IsRequired();
            builder.Property(t => t.order_date).IsRequired();
            builder.Property(t => t.status).IsRequired();
            builder.Property(t => t.total_amount).HasColumnType("numeric").IsRequired();
            builder.Property(t => t.customer_id).IsRequired();

            builder
            .HasOne(x => x.customer)
            .WithMany(y => y.Orders)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.customer_id)
            .OnDelete(DeleteBehavior.Restrict);

            builder
            .HasOne(x => x.payment_method)
            .WithMany(y => y.orders)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.payment_method_id)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}