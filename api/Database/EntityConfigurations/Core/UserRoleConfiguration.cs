using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("a_user_role", "public");

            builder.Property(t => t.role_cd).HasMaxLength(15).IsRequired();
            builder.Property(t => t.user_id).IsRequired();

            builder
            .HasOne(x => x.user)
            .WithOne(y => y.user_role)
            .HasForeignKey<UserRole>(z => z.user_id)
            .OnDelete(DeleteBehavior.Restrict);

            builder
            .HasOne(x => x.role)
            .WithMany(y => y.user_roles)
            .HasPrincipalKey(w => w.code)
            .HasForeignKey(z => z.role_cd)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}