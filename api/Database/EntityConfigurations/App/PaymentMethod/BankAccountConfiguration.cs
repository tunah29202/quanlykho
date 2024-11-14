using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations
{
    public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.ToTable("a_bank_account", "public");

            builder.Property(t => t.card_number).HasMaxLength(20).IsRequired();
            builder.Property(t => t.card_name).HasMaxLength(250).IsRequired();

            builder
            .HasOne(x => x.payment_method)
            .WithMany(y => y.bank_accounts)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.payment_method_id)
            .OnDelete(DeleteBehavior.Restrict);

            builder
            .HasOne(x => x.bank_name)
            .WithMany(y => y.bank_accounts)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.bank_name_id)
            .OnDelete(DeleteBehavior.Restrict);

            builder
            .HasOne(x => x.bank_branch)
            .WithMany(y => y.bank_accounts)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.bank_branch_id)
            .OnDelete(DeleteBehavior.Restrict);
            builder
            .HasOne(x => x.card_type)
            .WithMany(y => y.bank_accounts)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.card_type_id)
            .OnDelete(DeleteBehavior.Restrict);

        }
    }
}