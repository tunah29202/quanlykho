using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations
{
    public class BankBranchConfiguration : IEntityTypeConfiguration<BankBranch>
    {
        public void Configure(EntityTypeBuilder<BankBranch> builder)
        {
            builder.ToTable("a_bank_branch", "public");

            builder.Property(t => t.name).HasMaxLength(250).IsRequired();
        }
    }
}