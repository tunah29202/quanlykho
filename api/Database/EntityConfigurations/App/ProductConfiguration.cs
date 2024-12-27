using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("a_product", "dbo");

            builder.Property(t => t.name).HasMaxLength(250);
            builder.Property(t => t.image).HasMaxLength(250);
            builder.Property(t => t.origin).HasMaxLength(250);
            builder.Property(t => t.price_unit).HasMaxLength(20);
            builder.Property(t => t.ingredient).HasMaxLength(2048);
            builder.Property(t => t.warehouse_id);
            builder.Property(t => t.category_id);
            builder.Property(t => t.status);
            builder.Property(t => t.quantity);

            builder
            .HasOne(x => x.category)
            .WithMany(y => y.products)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.category_id)
            .OnDelete(DeleteBehavior.Restrict);

            builder
            .HasOne(x => x.warehouse)
            .WithMany(y => y.products)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.warehouse_id)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}