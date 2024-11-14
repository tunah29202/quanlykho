using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations
{
    public class BankNameConfiguration : IEntityTypeConfiguration<BankName>
    {
        public void Configure(EntityTypeBuilder<BankName> builder)
        {
            builder.ToTable("a_bank_name", "public");

            builder.Property(t => t.name).HasMaxLength(250).IsRequired();
        }
    }
}