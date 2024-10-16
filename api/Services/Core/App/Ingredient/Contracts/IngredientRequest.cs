using FluentValidation;
namespace Services.Core.Contracts
{
    public class IngredientRequest
    {
        public string category_id { get; set; }
        public string name { get; set; }
    }
    public class IngredientRequestValidator : AbstractValidator<IngredientRequest>
    {
        public IngredientRequestValidator()
        {
            RuleFor(_ => _.name).NotNull().NotEmpty().MaximumLength(2048);
        }
    }
}
