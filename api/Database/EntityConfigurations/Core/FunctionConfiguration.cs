using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations
{
    public class FunctionConfiguration : IEntityTypeConfiguration<Function>
    {
        public void Configure(EntityTypeBuilder<Function> builder)
        {
            builder.ToTable("a_function", "public");

            builder.Property(t => t.code).HasMaxLength(15).IsRequired();
            builder.Property(t => t.name).HasMaxLength(250);
            builder.Property(t => t.url).HasMaxLength(250);
            builder.Property(t => t.method).HasMaxLength(10);
            builder.Property(t => t.parent_cd).HasMaxLength(15);

            builder
            .HasOne(x => x.parent)
            .WithMany(y => y.children)
            .HasPrincipalKey(w => w.code)
            .HasForeignKey(z => z.parent_cd)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}