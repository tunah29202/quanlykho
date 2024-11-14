using FluentValidation;
namespace Services.Core.Contracts
{
    public class IngredientNameRequest
    {
        public string? name { get; set; }
    }
    public class IngredientNameRequestRequestValidator : AbstractValidator<IngredientNameRequest>
    {
        public IngredientNameRequestRequestValidator()
        {
            RuleFor(x => x.name).NotNull().NotEmpty().MaximumLength(20);
        }
    }
}
