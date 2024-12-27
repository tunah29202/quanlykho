using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable("a_warehouse", "dbo");

            builder.Property(t => t.code).HasMaxLength(30).IsRequired();
            builder.Property(t => t.name).HasMaxLength(250).IsRequired();
            builder.Property(t => t.address).HasMaxLength(250).IsRequired();
            builder.Property(t => t.tel).HasMaxLength(15);
        }
    }
}