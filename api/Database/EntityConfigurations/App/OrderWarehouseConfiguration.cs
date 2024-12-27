using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations
{
    public class OrderWarehouseConfiguration : IEntityTypeConfiguration<OrderWarehouse>
    {
        public void Configure(EntityTypeBuilder<OrderWarehouse> builder)
        {
            builder.ToTable("a_order_warehouse", "dbo");

            builder.Property(t => t.warehouse_id).IsRequired();
            builder.Property(t => t.order_id).IsRequired();

            builder
            .HasOne(x => x.warehouse)
            .WithMany(y => y.order_warehouses)
            .HasForeignKey(z => z.warehouse_id)
            .HasPrincipalKey(w => w.id)
            .OnDelete(DeleteBehavior.Restrict);

            builder
            .HasOne(x => x.order)
            .WithMany(y => y.order_warehouses)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.order_id)
            .OnDelete(DeleteBehavior.Restrict);

        }
    }
}