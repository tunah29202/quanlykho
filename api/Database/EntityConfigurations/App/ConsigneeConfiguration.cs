using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Database.Entities;

namespace Database.EntityConfigurations
{
    public class ConsigneeConfiguration : IEntityTypeConfiguration<Consignee>
    {
        public void Configure(EntityTypeBuilder<Consignee> builder)
        {
            builder.ToTable("a_consignee", "public");

            builder.Property(t => t.name).HasMaxLength(250).IsRequired();
            builder.Property(t => t.address).HasMaxLength(250).IsRequired();
            builder.Property(t => t.tax).HasMaxLength(15).IsRequired();
            builder.Property(t => t.email).HasMaxLength(250);
            builder.Property(t => t.tel).HasMaxLength(15).IsRequired();
            builder.Property(t => t.fax).HasMaxLength(15);
        }
    }
}