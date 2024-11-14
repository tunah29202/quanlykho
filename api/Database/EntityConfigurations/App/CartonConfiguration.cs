using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations
{
    public class CartonConfiguration : IEntityTypeConfiguration<Carton>
    {
        public void Configure(EntityTypeBuilder<Carton> builder)
        {
            builder.ToTable("a_carton", "public");

            builder.Property(t => t.carton_no).HasMaxLength(20).IsRequired();
            builder.Property(t => t.net_weight).HasColumnType("numeric").IsRequired();
            builder.Property(t => t.gross_weight).HasColumnType("numeric").IsRequired();
            builder.Property(t => t.length).HasColumnType("numeric").IsRequired();
            builder.Property(t => t.height).HasColumnType("numeric").IsRequired();
            builder.Property(t => t.width).HasColumnType("numeric").IsRequired();
            builder.Property(t => t.volume).HasColumnType("numeric").IsRequired();
            builder.Property(t => t.total_amount).HasColumnType("numeric").IsRequired();
            builder.Property(t => t.warehouse_id).IsRequired();

            builder
            .HasOne(x => x.warehouse)
            .WithMany(y => y.cartons)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.warehouse_id)
            .OnDelete(DeleteBehavior.Restrict);
            builder
            .HasOne(x => x.customer)
            .WithMany(y => y.cartons)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.customer_id)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}