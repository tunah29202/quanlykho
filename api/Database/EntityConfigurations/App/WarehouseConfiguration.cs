using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Database.Entities;

namespace Database.EntityConfigurations
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable("a_warehouse", "public");

            builder.Property(t => t.code).HasMaxLength(10).IsRequired();
            builder.Property(t => t.name).HasMaxLength(250).IsRequired();
            builder.Property(t => t.address).HasMaxLength(250).IsRequired();
            builder.Property(t => t.tel).HasMaxLength(15);
        }
    }
}