using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("a_role", "public");

            builder.Property(t => t.code).HasMaxLength(15).IsRequired();
            builder.Property(t => t.name).HasMaxLength(160).IsRequired();
            builder.Property(t => t.description).HasMaxLength(240);
        }
    }
}