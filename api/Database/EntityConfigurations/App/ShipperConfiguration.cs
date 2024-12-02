using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations
{
    public class ShipperConfiguration : IEntityTypeConfiguration<Shipper>
    {
        public void Configure(EntityTypeBuilder<Shipper> builder)
        {
            builder.ToTable("a_shipper", "public");

            builder.Property(t => t.name).HasMaxLength(250).IsRequired();
            builder.Property(t => t.address).HasMaxLength(250).IsRequired();
            builder.Property(t => t.tel).HasMaxLength(15).IsRequired();
            builder.Property(t => t.fax).HasMaxLength(30);
            builder.Property(t => t.email).HasMaxLength(250);
        }
    }
}