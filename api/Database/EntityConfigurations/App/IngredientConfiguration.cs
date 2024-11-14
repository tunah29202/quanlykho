using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EntityConfigurations
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.ToTable("a_ingredient", "public");

            builder.Property(t => t.category_id).IsRequired();
            builder.Property(t => t.name).HasMaxLength(2048).IsRequired();

            builder
            .HasOne(x => x.category)
            .WithMany(y => y.ingredients)
            .HasPrincipalKey(w => w.id)
            .HasForeignKey(z => z.category_id)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}