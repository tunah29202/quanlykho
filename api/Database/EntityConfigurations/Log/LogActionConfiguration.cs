using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations
{
    public class LogActionConfiguration : IEntityTypeConfiguration<LogAction>
    {
        public void Configure(EntityTypeBuilder<LogAction> builder)
        {
            builder.ToTable("m_log_action", "dbo");

            builder.Property(t => t.method).HasMaxLength(20);
            builder
               .HasOne(x => x.user)
               .WithMany(y => y.log_actions)
               .HasPrincipalKey(w => w.id)
               .HasForeignKey(z => z.user_id)
               .OnDelete(DeleteBehavior.Restrict)
               ;
        }
    }
}