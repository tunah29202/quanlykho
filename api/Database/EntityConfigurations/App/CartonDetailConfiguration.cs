using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Database.Entities;

namespace Database.EntityConfigurations
{
    public class CartonDetailConfiguration : IEntityTypeConfiguration<CartonDetail>
    {
        public void Configure(EntityTypeBuilder<CartonDetail> builder)
        {
            builder.ToTable("a_carton_detail", "public");

            builder.Property(t => t.carton_id).IsRequired();
            builder.Property(t => t.package_id).IsRequired();
            builder.Property(t => t.quantity).IsRequired();
            builder.Property(t => t.unit).IsRequired().HasMaxLength(20);

            builder
            .HasOne(x => x.carton)
            .WithMany(y => y.carton_details)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.carton_id)
            .OnDelete(DeleteBehavior.Restrict);

            builder
            .HasOne(x => x.package)
            .WithMany(y => y.carton_details)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.package_id)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}