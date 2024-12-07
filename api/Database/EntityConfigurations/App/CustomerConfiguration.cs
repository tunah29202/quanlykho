using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("a_customer", "public");

            builder.Property(t => t.name).HasMaxLength(250).IsRequired();
            builder.Property(t => t.address).HasMaxLength(250).IsRequired();
            builder.Property(t => t.tel).HasMaxLength(15);
        }
    }
}