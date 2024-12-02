using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("a_user", "public");
            builder.Property(t => t.code).HasMaxLength(30);
            builder.Property(t => t.user_name).HasMaxLength(100).IsRequired();
            builder.Property(t => t.full_name).HasMaxLength(100).IsRequired();
            builder.Property(t => t.gender).HasMaxLength(10).IsRequired();
            builder.Property(t => t.hash_password).HasMaxLength(100);
            builder.Property(t => t.salt).HasMaxLength(100);
            builder.Property(t => t.email).HasMaxLength(100).IsRequired();
            builder.Property(t => t.phone).HasMaxLength(30);

            builder
            .HasOne(x => x.customer)
            .WithOne(x => x.user)
            .HasForeignKey<User>(x => x.customer_id)
            .OnDelete(DeleteBehavior.Restrict);

        }
    }
}