using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations
{
    public class UserWarehouseConfiguration : IEntityTypeConfiguration<UserWarehouse>
    {
        public void Configure(EntityTypeBuilder<UserWarehouse> builder)
        {
            builder.ToTable("a_user_warehouse", "public");

            builder.Property(t => t.warehouse_id).IsRequired();
            builder.Property(t => t.user_id).IsRequired();

            builder
            .HasOne(x => x.warehouse)
            .WithMany(y => y.user_warehouses)
            .HasForeignKey(z => z.warehouse_id)
            .HasPrincipalKey(w => w.id)
            .OnDelete(DeleteBehavior.Restrict);

            builder
            .HasOne(x => x.user)
            .WithMany(y => y.user_warehouses)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.user_id)
            .OnDelete(DeleteBehavior.Restrict);

        }
    }
}