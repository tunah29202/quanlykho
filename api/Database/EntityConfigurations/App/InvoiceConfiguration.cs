using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("a_invoice", "public");

            builder.Property(t => t.invoice_no).HasMaxLength(20).IsRequired();
            builder.Property(t => t.invoice_date).HasMaxLength(20);
            builder.Property(t => t.shipped_per).HasMaxLength(250);
            builder.Property(t => t.shipped_date).HasMaxLength(20);
            builder.Property(t => t.total_weight).HasMaxLength(20);
            builder.Property(t => t.total_volumn).HasColumnType("decimal(18,2)");
            builder.Property(t => t.shipper_id).IsRequired();
            builder.Property(t => t.status).IsRequired();
            builder.Property(t => t.carton_id).IsRequired();
            builder.Property(t => t.order_id).IsRequired();
            builder.Property(t => t.payment_method_id).IsRequired();

            builder
            .HasOne(x => x.carton)
            .WithMany(y => y.invoices)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.carton_id)
            .OnDelete(DeleteBehavior.Restrict);

            builder
            .HasOne(x => x.shipper)
            .WithMany(y => y.invoices)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.shipper_id)
            .OnDelete(DeleteBehavior.Restrict);

            builder
            .HasOne(x => x.order)
            .WithOne(y => y.invoice)
            .HasForeignKey<Invoice>(z => z.order_id)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

            builder
            .HasOne(x => x.warehouse)
            .WithMany(y => y.invoices)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.warehouse_id)
            .OnDelete(DeleteBehavior.Restrict);

            builder
            .HasOne(x => x.payment_method)
            .WithMany(y => y.invoices)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.payment_method_id)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}