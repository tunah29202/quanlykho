using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Database.Entities;

namespace Database.EntityConfigurations
{
    public class PackageConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.ToTable("a_package", "public");

            builder.Property(t => t.name).HasMaxLength(250);
            builder.Property(t => t.origin).HasMaxLength(250);
            builder.Property(t => t.warehouse_id).IsRequired();
            builder.Property(t => t.quantity).IsRequired();
            builder.Property(t => t.note).HasMaxLength(2048);
            builder
            .HasOne(x => x.warehouse)
            .WithMany(y => y.packages)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.warehouse_id)
            .OnDelete(DeleteBehavior.Restrict);

            builder
            .HasOne(x => x.customer)
            .WithMany(y => y.packages)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.customer_id)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}