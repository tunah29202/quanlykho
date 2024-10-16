using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Database.Entities;

namespace Database.EntityConfigurations
{
    public class LogExceptionConfiguration : IEntityTypeConfiguration<LogException>
    {
        public void Configure(EntityTypeBuilder<LogException> builder)
        {
            builder.ToTable("m_log_exception", "public");

            builder.Property(t => t.function).HasMaxLength(200);
        }
    }
}