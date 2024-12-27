using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("a_role", "dbo");

            builder.Property(t => t.code).HasMaxLength(30).IsRequired();
            builder.Property(t => t.name).HasMaxLength(160).IsRequired();
            builder.Property(t => t.description).HasMaxLength(240);
        }
    }
}