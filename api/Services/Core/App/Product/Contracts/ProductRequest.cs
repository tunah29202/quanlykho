using FluentValidation;
namespace Services.Core.Contracts
{
    public class ProductRequest
    {
        public string? name { get; set; }
        public IFormFile? image { get; set; }
        public string? code { get; set; }
        public string? origin { get; set; }
        public decimal? price_unit { get; set; } = 0;
        public string? status { get; set; }
        public int? quantity { get; set; } = 0;
        public string? ingredient { get; set; }
        public Guid? warehouse_id { get; set; }
        public Guid? category_id { get; set; }
    }
    public class ProductRequestValidator : AbstractValidator<ProductRequest>
    {
        public ProductRequestValidator()
        {
            RuleFor(_ => _.name).MaximumLength(250);
            RuleFor(_ => _.origin).MaximumLength(250);
            RuleFor(_ => _.warehouse_id);
            RuleFor(_ => _.category_id);
            RuleFor(_ => _.status);
            RuleFor(_ => _.quantity);
        }
    }
}
