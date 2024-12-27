using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("a_permission", "dbo");

            builder.Property(t => t.role_cd).HasMaxLength(30).IsRequired();
            builder.Property(t => t.function_cd).HasMaxLength(30).IsRequired();

            builder
            .HasOne(x => x.role)
            .WithMany(y => y.permissions)
            .HasPrincipalKey(w => w.code)
            .HasForeignKey(z => z.role_cd)
            .OnDelete(DeleteBehavior.Restrict);

            builder
            .HasOne(x => x.function)
            .WithMany(y => y.permissions)
            .HasPrincipalKey(w => w.code)
            .HasForeignKey(z => z.function_cd)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}