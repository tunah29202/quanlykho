using FluentValidation;
namespace Services.Core.Contracts
{
    public class CategoryRequest
    {
        public string? name { get; set; }
        public List<IngredientNameRequest>? ingredient_names { get; set; }
    }
    public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
    {
        public CategoryRequestValidator()
        {
            RuleFor(x => x.name).NotNull().NotEmpty().MaximumLength(20);
        }
    }
}
