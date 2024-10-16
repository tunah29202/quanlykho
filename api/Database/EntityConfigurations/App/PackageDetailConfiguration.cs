using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Database.Entities;

namespace Database.EntityConfigurations
{
    public class PackageDetailConfiguration:IEntityTypeConfiguration<PackageDetail>
    {
        public void Configure(EntityTypeBuilder<PackageDetail> builder)
        {
            builder.ToTable("a_package_detail", "public");

            builder.Property(t => t.package_id).IsRequired();
            builder.Property(t => t.product_id).IsRequired();
            builder.Property(t => t.quantity).IsRequired();
            builder.Property(t => t.unit).IsRequired().HasMaxLength(20);

            builder
            .HasOne(x => x.product)
            .WithMany(y => y.package_details)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.product_id)
            .OnDelete(DeleteBehavior.Restrict);

            builder
            .HasOne(x => x.package)
            .WithMany(y => y.package_details)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.package_id)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}